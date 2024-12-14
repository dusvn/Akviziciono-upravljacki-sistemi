# External Battery Charging System

## Project Overview

This project involves the development and configuration of an external battery system designed to charge electronic devices. The battery features specific characteristics and functionalities essential for its operation, including various output ports, input charging options, power consumption rates, and charging rates. Additionally, the system supports communication through a configured protocol, and provides a user interface for monitoring and controlling the charging process.

## Battery Specifications

### Capacity (K)
- **Description**: The total capacity of the battery, which decreases with usage.

### Outputs
- **USB Ports**: 3 standard USB outputs (T1, T2, T3)
- **Socket**: 1 AC socket output (T4)
- **USB C Port**: 1 USB C output (T5)
- **Input**: 1 input for charging the battery

### Power Consumption Rates
- **USB Outputs (T1, T2, T3)**: Each USB output consumes 1% of the battery capacity per second when in use.
- **Socket Output (T4)**: Consumes 3% of the battery capacity per second when in use.
- **USB C Output (T5)**: Consumes 2% of the battery capacity per second when in use.

### Charging Rates
- **Power Supply I1**: Charges the battery at a rate of 3% per second.
- **Power Supply I2**: Charges the battery at a rate of 4% per second.

## Communication Protocol

- **RTU Slave Address**: 10
- **Transport Protocol**: TCP
- **Port**: 25252

## Output Definitions

### Configuration Requirements

- **A**: Scaling factor (integer, default value = 1)
- **B**: Offset (integer, default value = 0)
- **LowAlarm**: Upper limit for analog size in engineering units (integer, default value = 10)
- **EguMax**: Maximum battery capacity (default value = 100)
- **AbnormalValue**: Indicates abnormal state for digital sizes (default value = 100, opposite of normal state)

### Normal States
- **USB Outputs (T1-T3), Socket (T4), and Power Supply (I2)**: OFF
- **Power Supply (I1) and USB C Output**: ON

## Functional Requirements

### 1. **Communication Setup**
- Configure communication parameters in the `dCom` application and simulator to establish a TCP connection.

### 2. **Periodic Reading**
- Read all digital outputs (coils) periodically and refresh values on the user interface.
- Read all analog outputs (holding registers) periodically and refresh values on the user interface.

### 3. **Control Interface**
- Enable control for all defined digital outputs (coils) and refresh values on the user interface after successful commands.
- Enable control for all defined analog outputs (holding registers) and refresh values on the user interface after successful commands.

### 4. **Engineering Unit Conversion**
- Apply conversion to all output analog sizes using the provided formula.
- Convert engineering units back to raw values when issuing commands to output analog sizes.

### 5. **User Control**
- Users can remotely enable charging devices (T1-T5) only if the battery capacity is above the **LowAlarm** value.

### 6. **Alarm Handling**
- **Low Alarm**: If the battery capacity (K) falls below **LowAlarm**, report a LowAlarm, disable **T4** (socket) and **T5** (USB C), and enable power supplies **I1** and **I2** to recharge the battery.
- **Maximum Capacity**: If battery capacity (K) reaches **EguMax**, disable the power supply that was charging the battery (I1 or I2, or both).

### 7. **Simulation**
- Simulate the charging/discharging of the battery based on the devices and power supplies in use.

## Conclusion

The goal of this project is to create a robust and efficient external battery charging system with comprehensive monitoring and control capabilities. The outlined specifications and requirements will guide the development and configuration of the system to ensure seamless operation, user control, and performance monitoring.

---
