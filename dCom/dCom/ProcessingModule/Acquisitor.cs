using Common;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ProcessingModule
{
    /// <summary>
    /// Class containing logic for periodic polling.
    /// </summary>
    public class Acquisitor : IDisposable
    {
        private AutoResetEvent acquisitionTrigger; // sinhronizacija svega sto se koristi u ovom projektu 
        private IProcessingManager processingManager; // izvrsava operacije read/write 
        private Thread acquisitionWorker;
        private IStateUpdater stateUpdater;
        private IConfiguration configuration; // konfig sistema 

        /// <summary>
        /// Initializes a new instance of the <see cref="Acquisitor"/> class.
        /// </summary>
        /// <param name="acquisitionTrigger">The acquisition trigger.</param>
        /// <param name="processingManager">The processing manager.</param>
        /// <param name="stateUpdater">The state updater.</param>
        /// <param name="configuration">The configuration.</param>
		public Acquisitor(AutoResetEvent acquisitionTrigger, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration)
        {
            this.stateUpdater = stateUpdater;
            this.acquisitionTrigger = acquisitionTrigger;
            this.processingManager = processingManager;
            this.configuration = configuration;
            this.InitializeAcquisitionThread();
            this.StartAcquisitionThread();
        }

        #region Private Methods

        /// <summary>
        /// Initializes the acquisition thread.
        /// </summary>
        private void InitializeAcquisitionThread()
        {
            this.acquisitionWorker = new Thread(Acquisition_DoWork);
            this.acquisitionWorker.Name = "Acquisition thread";
        }

        /// <summary>
        /// Starts the acquisition thread.
        /// </summary>
		private void StartAcquisitionThread()
        {
            acquisitionWorker.Start();
        }

        /// <summary>
        /// Acquisitor thread logic.
        /// Metoda je prosledjena tredu i stalno ce se izvrsavati prikupljanje podataka i slanje na nas UI sa simulatora 
        /// </summary>
		private void Acquisition_DoWork()
        {
            
            List<IConfigItem> config_items = this.configuration.GetConfigurationItems(); // ucitava se konfig 

            while (true)
            {
                acquisitionTrigger.WaitOne();
                foreach (IConfigItem item in config_items)
                {
                    item.SecondsPassedSinceLastPoll++;
                    if (item.SecondsPassedSinceLastPoll == item.AcquisitionInterval)
                    {
                        processingManager.ExecuteReadCommand(item
                            , this.configuration.GetTransactionId(),
                            this.configuration.UnitAddress, item.StartAddress, item.NumberOfRegisters);
                        item.SecondsPassedSinceLastPoll = 0;
                    }
                }
            }
            

        }

        #endregion Private Methods

        /// <inheritdoc />
        public void Dispose()
        {
            acquisitionWorker.Abort();
        }
    }
}