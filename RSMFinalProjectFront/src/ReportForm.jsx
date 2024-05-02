import React, { useState } from 'react'
import axios from 'axios'
import './ReportForm.css'

const ReportForm = () => {

    //Initial state
    const [formData, setFormData] = useState({
        productCategory: '',
        productName: '',
        orderDate: '',
        pageNumber: 1,
        pageSize: 10,
    });

    const [tableData, setTableData] = useState([])
    const [isLoading, setIsLoading] = useState(false)
    const [showDataNotFound, setShowDataNotFound] = useState(false)

    /**
     * The handleChange function updates the form data with the new value based on
     * the input field name.
     */
    const handleChange = (event) => {
        const { name, value } = event.target
        setFormData({ ...formData, [name]: value })
    };

    /**
     * The `handleSubmit` function is an asynchronous function that handles form
     * submission, makes a GET request to a specified API endpoint to fetch sales
     * reports data, and updates the state variables accordingly.
     */
    const handleSubmit = async (event) => {
        event.preventDefault()
        setIsLoading(true)
        setShowDataNotFound(false)

        const apiUrl = import.meta.env.VITE_APP_API_URL

        try {
            const response = await axios.get(`${apiUrl}/SalesReports/GetAll`, {
                params: formData,
            });
            setTableData(response.data)
            if (response.data) {
                setShowDataNotFound(true)
            }
        } catch (error) {
            console.error(error)
        } finally {
            setIsLoading(false);
        }
    };

    /**
     * The `handleDownload` function is an asynchronous function that downloads a
     * PDF report from a specified API URL and handles the download process,
     * including error handling and setting loading indicators.
     */
    const handleDownload = async (event) => {
        event.preventDefault()
        setIsLoading(true)

        const apiUrl = import.meta.env.VITE_APP_API_URL

        try {
            const response = await axios.get(`${apiUrl}/SalesReports/GetPdfReport`, {
                responseType: 'blob', // Specify response type as blob for binary data
                params: formData,
            })

            const downloadLink = document.createElement('a')
            downloadLink.href = URL.createObjectURL(new Blob([response.data]))
            downloadLink.download = `SalesReport_${Date.now()}.pdf` // Download filename with .pdf extension
            downloadLink.click()

            URL.revokeObjectURL(downloadLink.href) // Revoke temporary URL after download
        } catch (error) {
            console.error('Error downloading report:', error)
            // Handle errors gracefully
        } finally {
            setIsLoading(false); // Set loading indicator to false, regardless of success or failure
        }
    };

    // Format date
    const formatDate = (dateString) => {
        const options = { year: 'numeric', month: '2-digit', day: '2-digit' };
        return new Date(dateString).toLocaleDateString(undefined, options);
    };

    // Money format
    const formatPrice = (price) => {
        return `$${price.toFixed(2)}`;
    };

    return (
        <form onSubmit={handleSubmit}>
            <div style={{ display: 'flex' }}>
                <div style={{ marginRight: '10px' }}>
                    <label htmlFor="pageNumber">Page Number:</label>
                    <select
                        id="pageNumber"
                        name="pageNumber"
                        value={formData.pageNumber}
                        onChange={handleChange}
                    >
                        {/* Add options for page numbers based on your requirements */}
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        {/* ... */}
                    </select>
                </div>
                <div style={{ marginRight: '10px' }}>
                    <label htmlFor="pageSize">Page Size:</label>
                    <select
                        id="pageSize"
                        name="pageSize"
                        value={formData.pageSize}
                        onChange={handleChange}
                    >
                        {/* Add options for page sizes based on your requirements */}
                        <option value="5">5</option>
                        <option value="10">10</option>
                        <option value="25">25</option>
                        <option value="50">50</option>
                        {/* ... */}
                    </select>
                </div>
                <br />
                <div>
                    <label htmlFor="productCategory">Product Category:</label>
                    <input
                        type="text"
                        id="productCategory"
                        name="productCategory"
                        value={formData.productCategory}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label htmlFor="productName">Product Name:</label>
                    <input
                        type="text"
                        id="productName"
                        name="productName"
                        value={formData.productName}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label htmlFor="orderDate">Order Date:</label>
                    <input
                        type="date"
                        id="orderDate"
                        name="orderDate"
                        value={formData.orderDate}
                        onChange={handleChange}
                    />
                </div>
                <button type="submit" disabled={isLoading}>
                    {isLoading ? 'Loading...' : 'Generate Report'}
                </button>
                <button type="button" onClick={handleDownload} disabled={isLoading}>
                    {isLoading ? 'Loading...' : 'Download Report'}
                </button>
            </div>

            {isLoading && (
                <div className="loading-indicator">
                    {/* Add your loading indicator content here (e.g., spinner, text) */}
                    <p>Fetching data...</p>
                </div>
            )}

            {/* {showDataNotFound && <p>Data not found.</p>} */}
            <div className="table-container">
                {tableData.length ? (
                    <table>
                        <thead>
                            <tr>
                                <th key="no">No.</th>
                                <th>Order ID</th>
                                <th>Order Date</th>
                                <th>Customer ID</th>
                                <th>Product ID</th>
                                <th>Product</th>
                                <th>Category</th>
                                <th>Unit Price</th>
                                <th>Quantity</th>
                                <th>Total Price</th>
                                <th>Person ID</th>
                                <th>Person</th>
                                <th>Shipping Address</th>
                                <th>Billing Address</th>
                            </tr>
                        </thead>
                        <tbody>
                            {tableData.map((row, index) => (
                                <tr key={row.id || index}>
                                    <td>{index + 1}</td>
                                    <td>{row.orderId}</td>
                                    <td>{formatDate(row.orderDate)}</td>
                                    <td>{row.customerId}</td>
                                    <td>{row.productId}</td>
                                    <td>{row.productName}</td>
                                    <td>{row.productCategory}</td>
                                    <td>{formatPrice(row.unitPrice)}</td>
                                    <td>{row.quantity}</td>
                                    <td>{formatPrice(row.totalPrice)}</td>
                                    <td>{row.salesPersonId}</td>
                                    <td>{row.firstName} {row.lastName}</td>
                                    <td>{row.shippingAddress}</td>
                                    <td>{row.billingAddress}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                ) : <p>No data</p>}
            </div>
            <br />
        </form>
    );
};

export default ReportForm;
