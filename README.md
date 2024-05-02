# RSM Final Project

## Setup Instructions

This repository contains two main components: the API and the user interface for the RSM final project.

### API Setup

1. **RSMFinalProjectAPI**: Navigate to the API directory.

2. Run `dotnet run`.

3. Upon startup, two views will be created in your database: 

   ```
   vSalesReport
   ```

    and 

   ```
   vSalesPerformance
   ```

   .

   - If you encounter any issues with this, please execute the provided scripts in the `DBScripts` folder.

### User Interface Setup

1. **RSMFinalProjectFront**: Navigate to the user interface directory.
2. Run `npm install`.
3. Before running the application, adjust the API URL in your environment variables. Set `VITE_APP_API_URL=http://localhost`.
4. Then, run `npm run dev`.

### Postman Testing

- Inside the `PostmanTest` folder, you'll find the collection needed to perform tests using Postman.