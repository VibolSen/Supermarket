using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Xps;
using System.Globalization;
using System.Linq;

namespace Supermarket
{
    public partial class ReceiptWindow : Window
    {
        public class ReceiptItem
        {
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Total { get; set; }
        }

        private ObservableCollection<ReceiptItem> _receiptItems;
        private string _employeeName;
        private string _customerName;

        public ReceiptWindow(List<Tuple<string, int, decimal>> items, string employeeName = "Unknown", string customerName = "Guest") // Added employeeName and customerName parameters
        {
            InitializeComponent();

            _employeeName = employeeName;
            _customerName = customerName;

            _receiptItems = new ObservableCollection<ReceiptItem>();

            decimal totalPrice = 0; // Calculate the total price

            foreach (var item in items)
            {
                decimal totalForItem = item.Item2 * item.Item3;
                _receiptItems.Add(new ReceiptItem
                {
                    ItemName = item.Item1,
                    Quantity = item.Item2,
                    Price = item.Item3,
                    Total = totalForItem
                });
                totalPrice += totalForItem;
            }

            ReceiptDataGrid.ItemsSource = _receiptItems;

            // Update header information
            EmployeeNameTextBlock.Text = $"Employee: {_employeeName}";
            CustomerNameTextBlock.Text = $"Customer: {_customerName}";
            DateTextBlock.Text = $"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}";

            // Update total price in footer
            TotalPriceTextBlock.Text = $"Total: {totalPrice.ToString("C", CultureInfo.CurrentCulture)}";
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument flowDocument = CreateFlowDocumentForPrinting();
                flowDocument.ColumnWidth = printDialog.PrintableAreaWidth;
                printDialog.PrintDocument(((IDocumentPaginatorSource)flowDocument).DocumentPaginator, "Receipt");
            }
        }

        private FlowDocument CreateFlowDocumentForPrinting()
        {
            FlowDocument flowDocument = new FlowDocument();

            // Add a title or header
            flowDocument.Blocks.Add(new Paragraph(new Run("Receipt")) { FontSize = 20, FontWeight = FontWeights.Bold, TextAlignment = TextAlignment.Center });
            flowDocument.Blocks.Add(new Paragraph(new Run($"Employee: {_employeeName}")) { FontSize = 12 });
            flowDocument.Blocks.Add(new Paragraph(new Run($"Customer: {_customerName}")) { FontSize = 12 });
            flowDocument.Blocks.Add(new Paragraph(new Run($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}")) { FontSize = 12 });
            flowDocument.Blocks.Add(new Paragraph(new Run("")) { FontSize = 12 }); // Add space

            //Create table
            Table table = new Table();
            table.CellSpacing = 0;

            // Set column widths
            table.Columns.Add(new TableColumn() { Width = new GridLength(1, GridUnitType.Star) }); // Item
            table.Columns.Add(new TableColumn() { Width = GridLength.Auto }); // Quantity
            table.Columns.Add(new TableColumn() { Width = GridLength.Auto }); // Price
            table.Columns.Add(new TableColumn() { Width = GridLength.Auto }); // Total
            table.RowGroups.Add(new TableRowGroup());

            // Add header row
            TableRow headerRow = new TableRow();
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Item")) { FontWeight = FontWeights.Bold }));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Quantity")) { FontWeight = FontWeights.Bold }));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Price")) { FontWeight = FontWeights.Bold }));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Total")) { FontWeight = FontWeights.Bold }));
            table.RowGroups[0].Rows.Add(headerRow);

            // Add data rows
            foreach (var item in _receiptItems)
            {
                TableRow row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.ItemName))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Quantity.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Price.ToString("C", CultureInfo.CurrentCulture))))); // Format as currency
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Total.ToString("C", CultureInfo.CurrentCulture))))); // Format as currency
                table.RowGroups[0].Rows.Add(row);
            }

            flowDocument.Blocks.Add(table);

            // Add the total price
            decimal total = _receiptItems.Sum(item => item.Total); // Calculate total from receipt items
            flowDocument.Blocks.Add(new Paragraph(new Run($"Total: {total.ToString("C", CultureInfo.CurrentCulture)}")) { TextAlignment = TextAlignment.Right, FontWeight = FontWeights.Bold });
            flowDocument.Blocks.Add(new Paragraph(new Run("Thank you for your purchase!")) { TextAlignment = TextAlignment.Center });

            return flowDocument;
        }


        private void SaveToPdfButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf|All files (*.*)|*.*";

            // Set default folder for saving PDFs
            string receiptsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Receipts");
            if (!Directory.Exists(receiptsFolder))
            {
                Directory.CreateDirectory(receiptsFolder);
            }
            saveFileDialog.InitialDirectory = receiptsFolder;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Create a FlowDocument
                    FlowDocument flowDocument = CreateFlowDocumentForPrinting();

                    // Create a temporary XPS file
                    string tempXpsFilePath = Path.GetTempFileName();

                    // Print to XPS
                    using (Package package = Package.Open(tempXpsFilePath, FileMode.Create))
                    {
                        XpsDocument doc = new XpsDocument(package);
                        XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                        writer.Write(((IDocumentPaginatorSource)flowDocument).DocumentPaginator);
                        doc.Close();
                    }

                    // Convert XPS to PDF
                    string tempPdfFilePath = Path.ChangeExtension(tempXpsFilePath, ".pdf");
                    PdfSharp.Xps.XpsConverter.Convert(tempXpsFilePath);

                    // Move the generated PDF to the user-specified location
                    File.Move(tempPdfFilePath, saveFileDialog.FileName);

                    // Clean up the temporary XPS file
                    File.Delete(tempXpsFilePath);

                    MessageBox.Show("Receipt saved to PDF successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving to PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}