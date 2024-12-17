
# USA States and Landmarks API  

## Overview  
The **USA States and Landmarks API** is a RESTful API built using **ASP.NET Core** that provides detailed information about U.S. states and their associated landmarks. This application allows users to retrieve, add, update, and delete state and landmark information seamlessly.  

It is designed to meet modern API standards and demonstrate best practices in backend development, including **repository patterns**, **DTO mappings**, and **cloud integrations**.

---

## Features  

- 📋 **Comprehensive State Information**:  
  - State name, capital, abbreviation, region, population, area, and time zone.  

- 🏛️ **Landmark Details**:  
  - Add and retrieve popular landmarks for each state.  

- 🔄 **CRUD Operations**:  
  - Perform Create, Read, Update, and Delete operations on states and landmarks.  

- 🌐 **RESTful Endpoints**:  
  - Clean, consistent, and easy-to-use API endpoints.  

- 🖼️ **State Flags**:  
  - Retrieve and display state flags stored in an **S3 bucket**.  

- ☁️ **Cloud Deployment**:  
  - Deployed to **AWS ECS** using Fargate for scalable and containerized deployment.  

---

## Technologies Used  

- **ASP.NET Core**: For building RESTful APIs.  
- **DynamoDB**: For flexible, serverless data storage.  
- **Amazon S3**: For hosting state flags and related media.  
- **Docker**: For containerization.  
- **AWS ECS Fargate**: For cloud-based deployment and scaling.  
- **AutoMapper**: For mapping DTOs and entities.  
