using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Web.UI;
using System.Web;
using System.Collections.Generic;

namespace gridviewpagewebform
{
    public partial class studentdata : System.Web.UI.Page
    {
        string cs = System.Configuration.ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Your page load logic here
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string fileExtension = Path.GetExtension(fileName);

                if (fileExtension.ToLower() == ".xlsx")
                {
                    // Specify the directory path where you want to save the uploaded files
                    string uploadDirectory = Server.MapPath("~/Uploads/");

                    // Check if the directory exists, if not, create it
                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }

                    // Combine the directory path and file name
                    string filePath = Path.Combine(uploadDirectory, fileName);

                    // Save the file to the server
                    FileUpload1.SaveAs(filePath);

                    // Read data from Excel file using EPPlus
                    DataTable dtExcelData = ReadExcelFile(filePath);

                    if (dtExcelData != null && dtExcelData.Rows.Count > 0)
                    {
                        // Insert data into the database
                        InsertDataIntoDatabase(dtExcelData);

                        // Display success message or update GridView
                        //lblMessage.Text = "Data imported successfully!";

                        // Bind the GridView
                        GridView1.DataBind(); // Assuming GridView1 is the ID of your GridView
                    }
                    else
                    {
                        // Handle the case when no data is retrieved from the Excel file
                        //lblMessage.Text = "No data found in the Excel file.";
                    }
                }
                else
                {
                    // Handle the case when the file format is not supported
                    //lblMessage.Text = "Invalid file format. Please select a valid Excel file.";
                }
            }
            else
            {
                // Handle the case when no file is selected
                //lblMessage.Text = "Please select an Excel file.";
            }
        }

        private DataTable ReadExcelFile(string filePath)
        {
            DataTable dtExcelData = new DataTable();

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // Assuming the first row contains column headers
                int totalCols = worksheet.Dimension.End.Column;
                int totalRows = worksheet.Dimension.End.Row;

                for (int col = 1; col <= totalCols; col++)
                {
                    dtExcelData.Columns.Add(worksheet.Cells[1, col].Text);
                }

                for (int row = 2; row <= totalRows; row++)
                {
                    DataRow dr = dtExcelData.NewRow();
                    for (int col = 1; col <= totalCols; col++)
                    {
                        dr[col - 1] = worksheet.Cells[row, col].Text;
                    }
                    dtExcelData.Rows.Add(dr);
                }
            }

            return dtExcelData;
        }

        private void InsertDataIntoDatabase(DataTable dt)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                foreach (DataRow row in dt.Rows)
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO studenttbl (name, rollnumber, phonenumber, address) VALUES (@name, @rollnumber, @phonenumber, @address)", con);
                    cmd.Parameters.AddWithValue("@name", row["name"]);
                    cmd.Parameters.AddWithValue("@rollnumber", row["rollnumber"]);
                    cmd.Parameters.AddWithValue("@phonenumber", row["phonenumber"]);
                    cmd.Parameters.AddWithValue("@address", row["address"]);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle the selection if needed
            // For demonstration purposes, export the selected row to Excel
            ExportToExcel(GridView1.SelectedRow);
        }

        private void ExportToExcel(GridViewRow selectedRow)
        {
            // Create a new DataTable and add columns
            DataTable dtExport = new DataTable();
            for (int i = 0; i < GridView1.Columns.Count; i++)
            {
                dtExport.Columns.Add();
            }

            // Add the selected row data to the DataTable
            DataRow dr = dtExport.NewRow();
            for (int i = 0; i < selectedRow.Cells.Count; i++)
            {
                dr[i] = selectedRow.Cells[i].Text;
            }
            dtExport.Rows.Add(dr);

            // Create Excel package using EPPlus
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Fill the worksheet with data
                worksheet.Cells["A1"].LoadFromDataTable(dtExport, true);

                // Save the Excel package to a MemoryStream
                MemoryStream ms = new MemoryStream();
                package.SaveAs(ms);

                // Clear the response content and set the appropriate headers
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", $"attachment;filename=SelectedRowData.xlsx");

                // Write the MemoryStream to the response output
                ms.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            // Existing code
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            // Existing code
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/AddNewRecord.aspx");
        }
      protected void btnDownloadExcel_Click(object sender, EventArgs e)
{
    // Get the GridView data with headers from the database
    DataTable dt = GetDataFromDatabase();

    // Create Excel package using EPPlus
    using (ExcelPackage package = new ExcelPackage())
    {
        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

        // Fill the worksheet with data
        worksheet.Cells["A1"].LoadFromDataTable(dt, true);

        // Remove the "id" column from the Excel worksheet
        int columnIndexToRemove = dt.Columns.IndexOf("id") + 1; // +1 because Excel column index is 1-based
        if (columnIndexToRemove > 0)
        {
            worksheet.DeleteColumn(columnIndexToRemove);
        }

        // Save the Excel package to a MemoryStream
        MemoryStream ms = new MemoryStream();
        package.SaveAs(ms);

        // Clear the response content and set the appropriate headers
        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("content-disposition", $"attachment;filename=GridViewData.xlsx");

        // Write the MemoryStream to the response output
        ms.WriteTo(Response.OutputStream);
        Response.Flush();
        Response.End();
    }
}

private DataTable GetDataFromDatabase()
{
    string cs = System.Configuration.ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
    string connectionString = cs;
    string tableName = "studenttbl";

    // Fetch column headers and data from the database
    string queryHeaders = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";

    List<string> headerData = new List<string>();
    DataTable dt = new DataTable();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        // Fetch column headers
        using (SqlCommand commandHeaders = new SqlCommand(queryHeaders, connection))
        {
            using (SqlDataReader readerHeaders = commandHeaders.ExecuteReader())
            {
                while (readerHeaders.Read())
                {
                    string columnName = readerHeaders.GetString(0);

                    // Exclude the "id" column
                    if (!columnName.Equals("id", StringComparison.OrdinalIgnoreCase))
                    {
                        headerData.Add(columnName);
                        dt.Columns.Add(columnName);
                    }
                }
            }
        }

        // Fetch data rows
        string queryData = $"SELECT * FROM {tableName}";

        using (SqlCommand commandData = new SqlCommand(queryData, connection))
        {
            using (SqlDataReader readerData = commandData.ExecuteReader())
            {
                while (readerData.Read())
                {
                    DataRow dr = dt.NewRow();

                    // Fetch data for each column
                    foreach (DataColumn column in dt.Columns)
                    {
                        string columnName = column.ColumnName;

                        // Exclude the "id" column
                        if (!columnName.Equals("id", StringComparison.OrdinalIgnoreCase))
                        {
                            dr[columnName] = readerData[columnName].ToString();
                        }
                    }

                    dt.Rows.Add(dr);
                }
            }
        }
    }

    return dt;
}


        protected void btnDownloadPDF_Click(object sender, EventArgs e)
        {
            // Create a new MemoryStream to store the PDF content
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Create a new PdfWriter instance, specifying the MemoryStream as the output stream
                using (PdfWriter writer = new PdfWriter(memoryStream))
                {
                    // Create a new PdfDocument
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        // Create a Document
                        Document document = new Document(pdf);

                        // Add content to the document

                        // Add a title
                        document.Add(new Paragraph("GridView Data"));

                        // Add a table to hold the GridView data
                        iText.Layout.Element.Table table = new iText.Layout.Element.Table(GridView1.Columns.Count);

                        // Add headers to the table using the HeaderText property
                        //foreach (DataControlField field in GridView1.Columns)
                        //{
                        //    table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(field.HeaderText)));
                        //}
                        // Replace the existing code for adding headers with the following
                        // Fetch column headers from the database
                        string cs = System.Configuration.ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
                        string connectionString = cs;
                        string tableName = "studenttbl";

                        // Fetch column headers and data from the database
                        string query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";

                        List<string> headerData = new List<string>();
                        List<List<string>> rowData = new List<List<string>>();

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // Fetch column headers
                            using (SqlCommand commandHeaders = new SqlCommand(query, connection))
                            {
                                using (SqlDataReader readerHeaders = commandHeaders.ExecuteReader())
                                {
                                    while (readerHeaders.Read())
                                    {
                                        headerData.Add(readerHeaders.GetString(0));
                                    }
                                }
                            }

                            // Fetch data rows
                            query = $"SELECT * FROM {tableName}";

                            using (SqlCommand commandData = new SqlCommand(query, connection))
                            {
                                using (SqlDataReader readerData = commandData.ExecuteReader())
                                {
                                    while (readerData.Read())
                                    {
                                        List<string> rowDataForCurrentRow = new List<string>();

                                        // Fetch data for each column
                                        for (int i = 0; i < readerData.FieldCount; i++)
                                        {
                                            rowDataForCurrentRow.Add(readerData[i].ToString());
                                        }

                                        rowData.Add(rowDataForCurrentRow);
                                    }
                                }
                            }
                        }

                        // Add headers to the table using the database header data
                        foreach (string header in headerData)
                        {
                            table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(header)));
                        }

                        // Add data rows to the table
                        foreach (List<string> row in rowData)
                        {
                            foreach (string cellValue in row)
                            {
                                table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(cellValue)));
                            }
                        }


                        // Add data rows to the table
                        foreach (GridViewRow row in GridView1.Rows)
                        {
                            foreach (TableCell cell in row.Cells)
                            {
                                
                            }
                            //foreach (TableCell cell in row.Cells)
                            //{
                            //    table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(cell.Text)));
                            //}
                        }

                        // Add the table to the document
                        document.Add(table);
                    }
                }

                // Clear the response content and set the appropriate headers
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=GridViewData.pdf");

                // Write the MemoryStream to the response output
                // Write the MemoryStream to the response output
                Response.BinaryWrite(memoryStream.ToArray());
                Response.Flush();

                // Complete the response
                HttpContext.Current.ApplicationInstance.CompleteRequest();



            }
        }


    }



}