using SNDDAL;
using SSS.Property.Setups;
using SSS.Property.Transactions;
using SSS.Property.Transactions.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SSS.DAL.Transactions
{
    public class LP_Purchase_Invoice_DAL : DBInteractionBase
    {
        private LP_P_Invoice_Property _objpinvoiceproperty;
        private LP_P_Invoice_Details _objInvoiceDetails;


        public LP_Purchase_Invoice_DAL()
        {

        }
        public LP_Purchase_Invoice_DAL(LP_P_Invoice_Property objpinvoiceproperty)
        {
            _objpinvoiceproperty = objpinvoiceproperty;
        }

        //Delete
        public bool Delete()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = @"dbo.[sp_PIDelete]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                //cmdToExecute.Parameters.Add(new SqlParameter("@companyIdx", SqlDbType.Int, 100, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objUserProperty.companyIdx));
                cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 100, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.idx));

                if (_mainConnectionIsCreatedLocal)
                {
                    // Open connection.
                    _mainConnection.Open();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }

                // Execute query.
                _rowsAffected = cmdToExecute.ExecuteNonQuery();
                // _errorCode = (Int32)cmdToExecute.Parameters["@iErrorCode"].Value;

                if (_errorCode != (int)LLBLError.AllOk)
                {
                    // Throw error.
                    throw new Exception("Stored Procedure 'sp_upate_branch' reported the ErrorCode: " + _errorCode);
                }

                return true;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("Branch::Update::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    // Close connection.
                    _mainConnection.Close();
                }
                cmdToExecute.Dispose();
            }
        }

        // Insert if ParentDocId > 0
        public override bool Insert() 
        {
            SqlCommand cmdToExecute = new SqlCommand();
            if (_objpinvoiceproperty.idx > 0)
            {
                //sp_PurchaseUpdate
                cmdToExecute.CommandText = "dbo.[sp_PInvoice_Update]";
            }
            else
            {
                cmdToExecute.CommandText = "dbo.[sp_PInvoice_Insert]";
            }

            cmdToExecute.CommandType = CommandType.StoredProcedure;

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                if (_objpinvoiceproperty.idx > 0)
                {
                    //    cmdToExecute.Parameters.Add(new SqlParameter("@poNumber", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.poNumber));
                    //    cmdToExecute.Parameters.Add(new SqlParameter("@vendorIdx", SqlDbType.Int, 50, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.vendorIdx));
                    //    cmdToExecute.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar, 80, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.description));
                    //    cmdToExecute.Parameters.Add(new SqlParameter("@totalamount", SqlDbType.Decimal, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.totalAmount));

                    //    cmdToExecute.Parameters.Add(new SqlParameter("@creationdate", SqlDbType.DateTime, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objPOMasterProperty.creationDate));

                       cmdToExecute.Parameters.Add(new SqlParameter("@createdBy", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                    //    cmdToExecute.Parameters.Add(new SqlParameter("@visible", SqlDbType.Int, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objPOMasterProperty.visible));
                    //    cmdToExecute.Parameters.Add(new SqlParameter("@status", SqlDbType.Int, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objPOMasterProperty.status));


                    //    cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.idx));
                    //    cmdToExecute.Parameters.Add(new SqlParameter("@MRNIdx", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.MRNIdx));

                    cmdToExecute.Parameters.Add(new SqlParameter("@InvoiceNo", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.Decimal, 50, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.TotalAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Decimal, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.NetAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.Status));
                    cmdToExecute.Parameters.Add(new SqlParameter("@PaymentType", SqlDbType.Int, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaymentType));
                    cmdToExecute.Parameters.Add(new SqlParameter("@IsPaid", SqlDbType.Bit, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.IsPaid));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Description", SqlDbType.Text, 20, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.Description));

                    cmdToExecute.Parameters.Add(new SqlParameter("@PaidAmount", SqlDbType.Decimal, 80, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@BalanceAmount", SqlDbType.Decimal, 9, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Taxable", SqlDbType.Bit, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.Taxable));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ParentDocID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.ParentDocID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@WarerHouseID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.WarerHouseID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@VendorID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));

                    cmdToExecute.Parameters.Add(new SqlParameter("@TaxAmount", SqlDbType.Decimal, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.TaxAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Reference", SqlDbType.Text, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.Reference));
                    cmdToExecute.Parameters.Add(new SqlParameter("@BankId", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BankId));
                    cmdToExecute.Parameters.Add(new SqlParameter("@AccountChequeNo", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.AccountChequeNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@IncometaxPercent", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ITAmount", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@whSalesTaxAmount", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.idx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@masterID", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.glIdx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@masterPId", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.masterPId));

                }
                else
                {
                    cmdToExecute.Parameters.Add(new SqlParameter("@InvoiceNo", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.Decimal, 50, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.TotalAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Decimal, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.NetAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                    cmdToExecute.Parameters.Add(new SqlParameter("@createdBy", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.Status));
                    cmdToExecute.Parameters.Add(new SqlParameter("@PaymentType", SqlDbType.Int, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaymentType));
                    cmdToExecute.Parameters.Add(new SqlParameter("@IsPaid", SqlDbType.Bit, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.IsPaid));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Description", SqlDbType.Text, 20, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.Description));

                    cmdToExecute.Parameters.Add(new SqlParameter("@PaidAmount", SqlDbType.Decimal, 80, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@BalanceAmount", SqlDbType.Decimal, 9, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Taxable", SqlDbType.Bit, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.Taxable));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ParentDocID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.ParentDocID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@WarerHouseID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.WarerHouseID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@VendorID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));

                    cmdToExecute.Parameters.Add(new SqlParameter("@TaxAmount", SqlDbType.Decimal, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.TaxAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Reference", SqlDbType.Text, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.Reference));
                    cmdToExecute.Parameters.Add(new SqlParameter("@BankId", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BankId));
                    cmdToExecute.Parameters.Add(new SqlParameter("@AccountChequeNo", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.AccountChequeNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@IncometaxPercent", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ITAmount", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@whSalesTaxAmount", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.idx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@glIdx", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.glIdx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@visible", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.visible));

                }

                if (_mainConnectionIsCreatedLocal)
                {

                    OpenConnection();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }

                this.StartTransaction();
                cmdToExecute.Transaction = this.Transaction;
                // Execute query.
                _rowsAffected = cmdToExecute.ExecuteNonQuery();
                //_iD = (Int32)cmdToExecute.Parameters["@iID"].Value;
                //_errorCode = cmdToExecute.Parameters["@ErrorCode"].Value;

                //Added By Ahsan
                if (_objpinvoiceproperty.idx > 0)
                {
                    var GLIDX = (Int32)cmdToExecute.Parameters["@masterID"].Value;
                    #region TAX DETAILS


                    if (_objpinvoiceproperty.TaxData != null)
                    {
                        foreach (DataRow row in _objpinvoiceproperty.TaxData.Rows)
                        {
                            row["PID"] = cmdToExecute.Parameters["@masterPId"].Value.ToString();
                        }
                        _objpinvoiceproperty.TaxData.AcceptChanges();

                        SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
                        _objpinvoiceproperty.TaxData.TableName = "PI_Taxes";

                        sbc.ColumnMappings.Clear();
                        sbc.ColumnMappings.Add("Tax_Id", "Tax_Id");
                        sbc.ColumnMappings.Add("PID", "PID");
                        sbc.ColumnMappings.Add("TaxPercent", "taxPercent");
                        
                        sbc.DestinationTableName = _objpinvoiceproperty.TaxData.TableName;
                        sbc.WriteToServer(_objpinvoiceproperty.TaxData);

                    }
                    if (_objpinvoiceproperty.InvoiceDetails != null)
                    {
                        foreach (DataRow row in _objpinvoiceproperty.InvoiceDetails.Rows)
                            row["PIIdx"] = cmdToExecute.Parameters["@masterPId"].Value.ToString();

                        _objpinvoiceproperty.InvoiceDetails.AcceptChanges();

                        SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
                        _objpinvoiceproperty.InvoiceDetails.TableName = "PI_Details";

                        sbc.ColumnMappings.Clear();
                        sbc.ColumnMappings.Add("ItemIdx", "ItemIdx");
                        sbc.ColumnMappings.Add("Qty", "Qty");
                        sbc.ColumnMappings.Add("PIIdx", "PIIdx");
                        sbc.ColumnMappings.Add("UnitPrice", "UnitPrice");
                        sbc.ColumnMappings.Add("TotalAmount", "TotalAmount");

                        sbc.DestinationTableName = _objpinvoiceproperty.InvoiceDetails.TableName;
                        sbc.WriteToServer(_objpinvoiceproperty.InvoiceDetails);

                    }
                    #endregion

                    #region Account Master Entry
                    //cmdToExecute.Parameters.Add(new SqlParameter("@masterID", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.glIdx));

                   // int GLIDX = (Int32)cmdToExecute.Parameters["@masterID"].Value;
                    //purchase entry for account gj same for all types
                    cmdToExecute = new SqlCommand();
                    // cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandText = "sp_InsertAccountGj";
                    cmdToExecute.Connection = _mainConnection;
                    cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                    cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                    cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 5));
                    cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.NetAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                    cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                    cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                    cmdToExecute.Transaction = this.Transaction;
                    _rowsAffected = cmdToExecute.ExecuteNonQuery();

                    //purchase end

                    //TAX entries
                    #region Account Gj Tax Entries
                    if (_objpinvoiceproperty.TaxData != null)
                    {
                        for (int i = 0; i < _objpinvoiceproperty.TaxData.Rows.Count; i++)
                        {

                            decimal taxpercntage = Convert.ToDecimal(_objpinvoiceproperty.TaxData.Rows[i]["TaxPercent"].ToString());
                            int taxid = Convert.ToInt32(_objpinvoiceproperty.TaxData.Rows[i]["Tax_Id"].ToString());
                            decimal taxamount = ((_objpinvoiceproperty.NetAmount / 100) * (taxpercntage));
                            cmdToExecute = new SqlCommand();
                            // cmdToExecute.CommandType = CommandType.StoredProcedure;
                            cmdToExecute.CommandType = CommandType.StoredProcedure;
                            cmdToExecute.CommandText = "sp_InsertAccountGj";
                            cmdToExecute.Connection = _mainConnection;
                            cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                            cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                            cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                            cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                            cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            //cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxid)); Yahan Tax ki coaIdx girna Chaye
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxid)); //sare tax k liye hardCodeValue hau
                            cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                            cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxamount));
                            cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                            cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                            cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                            cmdToExecute.Transaction = this.Transaction;
                            _rowsAffected = cmdToExecute.ExecuteNonQuery();
                        }
                    }
                    #endregion

                    #region on Payment
                    if (_objpinvoiceproperty.PaidAmount == 0)
                    {

                        ////cash entry bank
                        //cmdToExecute = new SqlCommand();
                        //// cmdToExecute.CommandType = CommandType.StoredProcedure;

                        //cmdToExecute.CommandType = CommandType.StoredProcedure;
                        //cmdToExecute.CommandText = "sp_InsertAccountGj";

                        //cmdToExecute.Connection = _mainConnection;
                        //cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        ////cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        //if (_objpinvoiceproperty.PaymentType == 1)
                        //{
                        //    //cash
                        //    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        //}
                        //if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        //{
                        //    //bank
                        //    // var data= _objpinvoicepropert
                        //    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        //}
                        //cmdToExecute.Transaction = this.Transaction;
                        //_rowsAffected = cmdToExecute.ExecuteNonQuery();


                        //acount payable
                        cmdToExecute = new SqlCommand();
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));
                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorCoaidx));
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();
                    }
                    #endregion

                    #region Full Payment

                    else if (_objpinvoiceproperty.BalanceAmount == 0)
                    {

                        //cash entry bank
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;

                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";

                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        if (_objpinvoiceproperty.PaymentType == 1)
                        {
                            //cash
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        }
                        if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        {
                            //bank
                            // var data= _objpinvoicepropert
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        }



                        cmdToExecute.Transaction = this.Transaction;
                        //this.StartTransaction();
                        // cmdToExecute.Transaction = this.Transaction;
                        // Execute query.
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();

                        //cash end


                    }


                    #endregion


                    #region partial Payment
                    else if ((_objpinvoiceproperty.PaidAmount != _objpinvoiceproperty.TotalAmount) && _objpinvoiceproperty.PaidAmount != 0)
                    {

                        //cash entry bank
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;

                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";

                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        if (_objpinvoiceproperty.PaymentType == 1)
                        {
                            //cash
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        }
                        if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        {
                            //bank
                            // var data= _objpinvoicepropert
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        }
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();


                        //acount payable
                        cmdToExecute = new SqlCommand();
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));
                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorCoaidx));
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();

                        #endregion





                        #endregion
                    }
                    else
                    {
                        if (_errorCode != (int)LLBLError.AllOk)
                        {
                            // Throw error.
                            this.RollBack();
                            throw new Exception("Stored Procedure 'sp_TRANSACTION_MASTER_Insert' reported the ErrorCode: " + _errorCode);

                        }
                    }
                }
                else
                {
                    #region TAX DETAILS


                    if (_objpinvoiceproperty.TaxData != null)
                    {
                        foreach (DataRow row in _objpinvoiceproperty.TaxData.Rows)
                        {
                            row["PID"] = cmdToExecute.Parameters["@ID"].Value.ToString();
                        }
                        _objpinvoiceproperty.TaxData.AcceptChanges();

                        SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
                        _objpinvoiceproperty.TaxData.TableName = "PI_Taxes";

                        sbc.ColumnMappings.Clear();
                        sbc.ColumnMappings.Add("Tax_Id", "Tax_Id");
                        sbc.ColumnMappings.Add("PID", "PID");
                        sbc.ColumnMappings.Add("TaxPercent", "taxPercent");
                        //sbc.ColumnMappings.Add(2, 1);
                        //sbc.ColumnMappings.Add("productTypeIdx", "productTypeIdx");
                        //sbc.ColumnMappings.Add("itemIdx", "itemIdx");
                        //sbc.ColumnMappings.Add("unitPrice", "unitPrice");
                        //sbc.ColumnMappings.Add("qty", "qty");
                        //sbc.ColumnMappings.Add("amount", "amount");
                        //sbc.ColumnMappings.Add("qty", "openItem");
                        //sbc.ColumnMappings.Add("Product_Code", "Product_Code");
                        //sbc.ColumnMappings.Add("Product", "Product_Name");
                        //sbc.ColumnMappings.Add("Status", "Status");

                        //sbc.ColumnMappings.Add("Department_Id", "Department_Id");
                        //sbc.ColumnMappings.Add("Description", "Description");

                        sbc.DestinationTableName = _objpinvoiceproperty.TaxData.TableName;
                        sbc.WriteToServer(_objpinvoiceproperty.TaxData);

                    }
                    if (_objpinvoiceproperty.InvoiceDetails != null)
                    {
                        foreach (DataRow row in _objpinvoiceproperty.InvoiceDetails.Rows)
                            row["PIIdx"] = cmdToExecute.Parameters["@ID"].Value.ToString();

                        _objpinvoiceproperty.InvoiceDetails.AcceptChanges();

                        SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
                        _objpinvoiceproperty.InvoiceDetails.TableName = "PI_Details";

                        sbc.ColumnMappings.Clear();
                        sbc.ColumnMappings.Add("ItemIdx", "ItemIdx");
                        sbc.ColumnMappings.Add("Qty", "Qty");
                        sbc.ColumnMappings.Add("PIIdx", "PIIdx");
                        sbc.ColumnMappings.Add("UnitPrice", "UnitPrice");
                        sbc.ColumnMappings.Add("TotalAmount", "TotalAmount");
                        //sbc.ColumnMappings.Add("Status", "Status");
                        //sbc.ColumnMappings.Add(2, 1);
                        //sbc.ColumnMappings.Add("productTypeIdx", "productTypeIdx");
                        //sbc.ColumnMappings.Add("itemIdx", "itemIdx");
                        //sbc.ColumnMappings.Add("unitPrice", "unitPrice");
                        //sbc.ColumnMappings.Add("qty", "qty");
                        //sbc.ColumnMappings.Add("amount", "amount");
                        //sbc.ColumnMappings.Add("qty", "openItem");
                        //sbc.ColumnMappings.Add("Product_Code", "Product_Code");
                        //sbc.ColumnMappings.Add("Product", "Product_Name");
                        //sbc.ColumnMappings.Add("Status", "Status");

                        //sbc.ColumnMappings.Add("Department_Id", "Department_Id");
                        //sbc.ColumnMappings.Add("Description", "Description");

                        sbc.DestinationTableName = _objpinvoiceproperty.InvoiceDetails.TableName;
                        sbc.WriteToServer(_objpinvoiceproperty.InvoiceDetails);

                    }
                    #endregion

                    #region Account Master Entry
                    cmdToExecute.Parameters.Add(new SqlParameter("@glIdx", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.glIdx));

                    int GLIDX = (Int32)cmdToExecute.Parameters["@glIdx"].Value;
                    //purchase entry for account gj same for all types
                    cmdToExecute = new SqlCommand();
                    // cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandText = "sp_InsertAccountGj";
                    cmdToExecute.Connection = _mainConnection;
                    cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                    cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                    cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 5));
                    cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.NetAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                    cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                    cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                    cmdToExecute.Transaction = this.Transaction;
                    _rowsAffected = cmdToExecute.ExecuteNonQuery();

                    //purchase end

                    //TAX entries
                    #region Account Gj Tax Entries
                    if (_objpinvoiceproperty.TaxData != null)
                    {
                        for (int i = 0; i < _objpinvoiceproperty.TaxData.Rows.Count; i++)
                        {

                            decimal taxpercntage = Convert.ToDecimal(_objpinvoiceproperty.TaxData.Rows[i]["TaxPercent"].ToString());
                            int taxid = Convert.ToInt32(_objpinvoiceproperty.TaxData.Rows[i]["Tax_Id"].ToString());
                            decimal taxamount = ((_objpinvoiceproperty.NetAmount / 100) * (taxpercntage));
                            cmdToExecute = new SqlCommand();
                            // cmdToExecute.CommandType = CommandType.StoredProcedure;
                            cmdToExecute.CommandType = CommandType.StoredProcedure;
                            cmdToExecute.CommandText = "sp_InsertAccountGj";
                            cmdToExecute.Connection = _mainConnection;
                            cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                            cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                            cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                            cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                            cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            //cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxid)); Yahan Tax ki coaIdx girna Chaye
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxid)); //sare tax k liye hardCodeValue hau
                            cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                            cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxamount));
                            cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                            cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                            cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                            cmdToExecute.Transaction = this.Transaction;
                            _rowsAffected = cmdToExecute.ExecuteNonQuery();
                        }
                    }
                    #endregion

                    #region on Payment
                    if (_objpinvoiceproperty.PaidAmount == 0)
                    {

                        ////cash entry bank
                        //cmdToExecute = new SqlCommand();
                        //// cmdToExecute.CommandType = CommandType.StoredProcedure;

                        //cmdToExecute.CommandType = CommandType.StoredProcedure;
                        //cmdToExecute.CommandText = "sp_InsertAccountGj";

                        //cmdToExecute.Connection = _mainConnection;
                        //cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        ////cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        //if (_objpinvoiceproperty.PaymentType == 1)
                        //{
                        //    //cash
                        //    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        //}
                        //if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        //{
                        //    //bank
                        //    // var data= _objpinvoicepropert
                        //    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        //}
                        //cmdToExecute.Transaction = this.Transaction;
                        //_rowsAffected = cmdToExecute.ExecuteNonQuery();


                        //acount payable
                        cmdToExecute = new SqlCommand();
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));
                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorCoaidx));
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();
                    }
                    #endregion

                    #region Full Payment

                    else if (_objpinvoiceproperty.BalanceAmount == 0)
                    {

                        //cash entry bank
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;

                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";

                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        if (_objpinvoiceproperty.PaymentType == 1)
                        {
                            //cash
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        }
                        if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        {
                            //bank
                            // var data= _objpinvoicepropert
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        }



                        cmdToExecute.Transaction = this.Transaction;
                        //this.StartTransaction();
                        // cmdToExecute.Transaction = this.Transaction;
                        // Execute query.
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();

                        //cash end


                    }


                    #endregion


                    #region partial Payment
                    else if ((_objpinvoiceproperty.PaidAmount != _objpinvoiceproperty.TotalAmount) && _objpinvoiceproperty.PaidAmount != 0)
                    {

                        //cash entry bank
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;

                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";

                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        if (_objpinvoiceproperty.PaymentType == 1)
                        {
                            //cash
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        }
                        if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        {
                            //bank
                            // var data= _objpinvoicepropert
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        }
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();


                        //acount payable
                        cmdToExecute = new SqlCommand();
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));
                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorCoaidx));
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();

                        #endregion





                        #endregion
                    }
                    else
                    {
                        if (_errorCode != (int)LLBLError.AllOk)
                        {
                            // Throw error.
                            this.RollBack();
                            throw new Exception("Stored Procedure 'sp_TRANSACTION_MASTER_Insert' reported the ErrorCode: " + _errorCode);

                        }
                    }
                }

                
                this.Commit();
                return true;
            }
            catch (Exception ex)
            {
                this.RollBack();
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("TRANSACTION_MASTER::Insert::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    //// Close connection.
                    //_mainConnection.Close();
                    CloseConnection();
                }
                cmdToExecute.Dispose();
            }
        }


        public  bool InsertDP()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            if (_objpinvoiceproperty.idx > 0)
            {
                //sp_PurchaseUpdate
                cmdToExecute.CommandText = "dbo.[sp_PInvoice_Update]";
            }
            else
            {
                cmdToExecute.CommandText = "dbo.[sp_PInvoice_Insert]";
            }

            cmdToExecute.CommandType = CommandType.StoredProcedure;

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                if (_objpinvoiceproperty.idx > 0)
                {
                    cmdToExecute.Parameters.Add(new SqlParameter("@createdBy", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                    cmdToExecute.Parameters.Add(new SqlParameter("@InvoiceNo", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.Decimal, 50, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.TotalAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Decimal, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.NetAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.Status));
                    cmdToExecute.Parameters.Add(new SqlParameter("@PaymentType", SqlDbType.Int, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaymentType));
                    cmdToExecute.Parameters.Add(new SqlParameter("@IsPaid", SqlDbType.Bit, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.IsPaid));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Description", SqlDbType.Text, 20, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.Description));

                    cmdToExecute.Parameters.Add(new SqlParameter("@PaidAmount", SqlDbType.Decimal, 80, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@BalanceAmount", SqlDbType.Decimal, 9, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Taxable", SqlDbType.Bit, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.Taxable));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ParentDocID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.ParentDocID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@WarerHouseID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.WarerHouseID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@VendorID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));

                    cmdToExecute.Parameters.Add(new SqlParameter("@TaxAmount", SqlDbType.Decimal, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.TaxAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Reference", SqlDbType.Text, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.Reference));
                    cmdToExecute.Parameters.Add(new SqlParameter("@BankId", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BankId));
                    cmdToExecute.Parameters.Add(new SqlParameter("@AccountChequeNo", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.AccountChequeNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@IncometaxPercent", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ITAmount", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@whSalesTaxAmount", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.idx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@masterID", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.glIdx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@masterPId", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.masterPId));

                }
                else
                {
                    cmdToExecute.Parameters.Add(new SqlParameter("@InvoiceNo", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.Decimal, 50, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.TotalAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Decimal, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.NetAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                    cmdToExecute.Parameters.Add(new SqlParameter("@createdBy", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.Status));
                    cmdToExecute.Parameters.Add(new SqlParameter("@PaymentType", SqlDbType.Int, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaymentType));
                    cmdToExecute.Parameters.Add(new SqlParameter("@IsPaid", SqlDbType.Bit, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.IsPaid));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Description", SqlDbType.Text, 20, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.Description));

                    cmdToExecute.Parameters.Add(new SqlParameter("@PaidAmount", SqlDbType.Decimal, 80, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@BalanceAmount", SqlDbType.Decimal, 9, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Taxable", SqlDbType.Bit, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.Taxable));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ParentDocID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.ParentDocID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@WarerHouseID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.WarerHouseID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@VendorID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));

                    cmdToExecute.Parameters.Add(new SqlParameter("@TaxAmount", SqlDbType.Decimal, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.TaxAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@Reference", SqlDbType.Text, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.Reference));
                    cmdToExecute.Parameters.Add(new SqlParameter("@BankId", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BankId));
                    cmdToExecute.Parameters.Add(new SqlParameter("@AccountChequeNo", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.AccountChequeNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@IncometaxPercent", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ITAmount", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@whSalesTaxAmount", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.idx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@glIdx", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.glIdx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@visible", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.visible));

                }

                if (_mainConnectionIsCreatedLocal)
                {

                    OpenConnection();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }

                this.StartTransaction();
                cmdToExecute.Transaction = this.Transaction;
                // Execute query.
                _rowsAffected = cmdToExecute.ExecuteNonQuery();

                if (_objpinvoiceproperty.idx > 0)
                {
                    //var piIdx = cmdToExecute.Parameters["@ID"].Value.ToString();
                    var GLIDX = (Int32)cmdToExecute.Parameters["@masterID"].Value;
                    var piIdx = (Int32)cmdToExecute.Parameters["@masterPId"].Value;
                    #region TAX DETAILS


                    if (_objpinvoiceproperty.TaxData != null)
                    {
                        foreach (DataRow row in _objpinvoiceproperty.TaxData.Rows)
                        {
                            row["PID"] = cmdToExecute.Parameters["@masterPId"].Value.ToString();
                        }
                        _objpinvoiceproperty.TaxData.AcceptChanges();

                        SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
                        _objpinvoiceproperty.TaxData.TableName = "PI_Taxes";

                        sbc.ColumnMappings.Clear();
                        sbc.ColumnMappings.Add("Tax_Id", "Tax_Id");
                        sbc.ColumnMappings.Add("PID", "PID");
                        sbc.ColumnMappings.Add("TaxPercent", "taxPercent");

                        sbc.DestinationTableName = _objpinvoiceproperty.TaxData.TableName;
                        sbc.WriteToServer(_objpinvoiceproperty.TaxData);

                    }
                    if (_objpinvoiceproperty.InvoiceDetails != null)
                    {
                        foreach (DataRow row in _objpinvoiceproperty.InvoiceDetails.Rows)
                            row["PIIdx"] = cmdToExecute.Parameters["@masterPId"].Value.ToString();

                        _objpinvoiceproperty.InvoiceDetails.AcceptChanges();

                        SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
                        _objpinvoiceproperty.InvoiceDetails.TableName = "PI_Details";

                        sbc.ColumnMappings.Clear();
                        sbc.ColumnMappings.Add("ItemIdx", "ItemIdx");
                        sbc.ColumnMappings.Add("Qty", "Qty");
                        sbc.ColumnMappings.Add("PIIdx", "PIIdx");
                        sbc.ColumnMappings.Add("UnitPrice", "UnitPrice");
                        sbc.ColumnMappings.Add("TotalAmount", "TotalAmount");

                        sbc.DestinationTableName = _objpinvoiceproperty.InvoiceDetails.TableName;
                        sbc.WriteToServer(_objpinvoiceproperty.InvoiceDetails);

                    }
                    #endregion

                    #region Inventory Log and Inventory

                    for (int i = 0; i < _objpinvoiceproperty.InvoiceDetails.Rows.Count; i++)
                    {
                        cmdToExecute = new SqlCommand();
                        cmdToExecute.CommandText = "sp_InventoryMaster";
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.Connection = _mainConnection;


                        cmdToExecute.Parameters.Add(new SqlParameter("@productid", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, Convert.ToInt16(_objpinvoiceproperty.InvoiceDetails.Rows[i]["itemIdx"])));
                        cmdToExecute.Parameters.Add(new SqlParameter("@newqty", SqlDbType.Decimal, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(_objpinvoiceproperty.InvoiceDetails.Rows[i]["Qty"])));
                        cmdToExecute.Parameters.Add(new SqlParameter("@newrate", SqlDbType.Decimal, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(_objpinvoiceproperty.InvoiceDetails.Rows[i]["UnitPrice"])));
                        cmdToExecute.Parameters.Add(new SqlParameter("@wareHouseIdx", SqlDbType.Decimal, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, (_objpinvoiceproperty.WarerHouseID)));

                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();
                    }

                    cmdToExecute = new SqlCommand();
                    // cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandText = "dbo.[sp_ProcessPIForLogs]";
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.Connection = _mainConnection;
                    cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, piIdx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@WarehouseIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.WarerHouseID));
                    //cmdToExecute.Parameters.Add(new SqlParameter("@WarehouseIdx", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed,));
                    cmdToExecute.Transaction = this.Transaction;
                    _rowsAffected = cmdToExecute.ExecuteNonQuery();

                    #endregion


                    #region Account Master Entry
                    //                    cmdToExecute.Parameters.Add(new SqlParameter("@masterID", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.glIdx));

                    //  int GLIDX = (Int32)cmdToExecute.Parameters["@masterID"].Value;
                    //purchase entry for account gj same for all types
                    cmdToExecute = new SqlCommand();
                    // cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandText = "sp_InsertAccountGj";
                    cmdToExecute.Connection = _mainConnection;
                    cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                    cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                    cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 5));
                    cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.NetAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                    cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                    cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                    cmdToExecute.Transaction = this.Transaction;
                    _rowsAffected = cmdToExecute.ExecuteNonQuery();

                    //purchase end

                    //TAX entries
                    #region Account Gj Tax Entries
                    if (_objpinvoiceproperty.TaxData != null)
                    {
                        for (int i = 0; i < _objpinvoiceproperty.TaxData.Rows.Count; i++)
                        {

                            decimal taxpercntage = Convert.ToDecimal(_objpinvoiceproperty.TaxData.Rows[i]["TaxPercent"].ToString());
                            int taxid = Convert.ToInt32(_objpinvoiceproperty.TaxData.Rows[i]["Tax_Id"].ToString());
                            decimal taxamount = ((_objpinvoiceproperty.NetAmount / 100) * (taxpercntage));
                            cmdToExecute = new SqlCommand();
                            // cmdToExecute.CommandType = CommandType.StoredProcedure;
                            cmdToExecute.CommandType = CommandType.StoredProcedure;
                            cmdToExecute.CommandText = "sp_InsertAccountGj";
                            cmdToExecute.Connection = _mainConnection;
                            cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                            cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                            cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                            cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                            cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            //cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxid)); Yahan Tax ki coaIdx girna Chaye
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxid)); //sare tax k liye hardCodeValue hau
                            cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                            cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxamount));
                            cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                            cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                            cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                            cmdToExecute.Transaction = this.Transaction;
                            _rowsAffected = cmdToExecute.ExecuteNonQuery();
                        }
                    }
                    #endregion

                    #region on Payment
                    if (_objpinvoiceproperty.PaidAmount == 0)
                    {

                        ////cash entry bank
                        //cmdToExecute = new SqlCommand();
                        //// cmdToExecute.CommandType = CommandType.StoredProcedure;

                        //cmdToExecute.CommandType = CommandType.StoredProcedure;
                        //cmdToExecute.CommandText = "sp_InsertAccountGj";

                        //cmdToExecute.Connection = _mainConnection;
                        //cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        ////cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        //if (_objpinvoiceproperty.PaymentType == 1)
                        //{
                        //    //cash
                        //    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        //}
                        //if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        //{
                        //    //bank
                        //    // var data= _objpinvoicepropert
                        //    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        //}
                        //cmdToExecute.Transaction = this.Transaction;
                        //_rowsAffected = cmdToExecute.ExecuteNonQuery();


                        //acount payable
                        cmdToExecute = new SqlCommand();
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));
                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorCoaidx));
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();
                    }
                    #endregion

                    #region Full Payment

                    else if (_objpinvoiceproperty.BalanceAmount == 0)
                    {

                        //cash entry bank
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;

                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";

                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        if (_objpinvoiceproperty.PaymentType == 1)
                        {
                            //cash
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        }
                        if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        {
                            //bank
                            // var data= _objpinvoicepropert
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        }



                        cmdToExecute.Transaction = this.Transaction;
                        //this.StartTransaction();
                        // cmdToExecute.Transaction = this.Transaction;
                        // Execute query.
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();

                        //cash end


                    }


                    #endregion


                    #region partial Payment
                    else if ((_objpinvoiceproperty.PaidAmount != _objpinvoiceproperty.TotalAmount) && _objpinvoiceproperty.PaidAmount != 0)
                    {

                        //cash entry bank
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;

                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";

                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        if (_objpinvoiceproperty.PaymentType == 1)
                        {
                            //cash
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        }
                        if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        {
                            //bank
                            // var data= _objpinvoicepropert
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        }
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();


                        //acount payable
                        cmdToExecute = new SqlCommand();
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));
                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorCoaidx));
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();

                        #endregion





                        #endregion
                    }
                    else
                    {
                        if (_errorCode != (int)LLBLError.AllOk)
                        {
                            // Throw error.
                            this.RollBack();
                            throw new Exception("Stored Procedure 'sp_TRANSACTION_MASTER_Insert' reported the ErrorCode: " + _errorCode);

                        }
                    }
                }
                else
                {
                    var piIdx = cmdToExecute.Parameters["@ID"].Value.ToString();
                    var GLIDX = cmdToExecute.Parameters["@glIdx"].Value.ToString();

                    #region TAX DETAILS


                    if (_objpinvoiceproperty.TaxData != null)
                    {
                        foreach (DataRow row in _objpinvoiceproperty.TaxData.Rows)
                        {
                            row["PID"] = cmdToExecute.Parameters["@ID"].Value.ToString();
                        }
                        _objpinvoiceproperty.TaxData.AcceptChanges();

                        SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
                        _objpinvoiceproperty.TaxData.TableName = "PI_Taxes";

                        sbc.ColumnMappings.Clear();
                        sbc.ColumnMappings.Add("Tax_Id", "Tax_Id");
                        sbc.ColumnMappings.Add("PID", "PID");
                        sbc.ColumnMappings.Add("TaxPercent", "taxPercent");

                        sbc.DestinationTableName = _objpinvoiceproperty.TaxData.TableName;
                        sbc.WriteToServer(_objpinvoiceproperty.TaxData);

                    }
                    if (_objpinvoiceproperty.InvoiceDetails != null)
                    {
                        foreach (DataRow row in _objpinvoiceproperty.InvoiceDetails.Rows)
                            row["PIIdx"] = cmdToExecute.Parameters["@ID"].Value.ToString();

                        _objpinvoiceproperty.InvoiceDetails.AcceptChanges();

                        SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
                        _objpinvoiceproperty.InvoiceDetails.TableName = "PI_Details";

                        sbc.ColumnMappings.Clear();
                        sbc.ColumnMappings.Add("ItemIdx", "ItemIdx");
                        sbc.ColumnMappings.Add("Qty", "Qty");
                        sbc.ColumnMappings.Add("PIIdx", "PIIdx");
                        sbc.ColumnMappings.Add("UnitPrice", "UnitPrice");
                        sbc.ColumnMappings.Add("TotalAmount", "TotalAmount");
                        //sbc.ColumnMappings.Add("Status", "Status");
                        //sbc.ColumnMappings.Add(2, 1);
                        //sbc.ColumnMappings.Add("productTypeIdx", "productTypeIdx");
                        //sbc.ColumnMappings.Add("itemIdx", "itemIdx");
                        //sbc.ColumnMappings.Add("unitPrice", "unitPrice");
                        //sbc.ColumnMappings.Add("qty", "qty");
                        //sbc.ColumnMappings.Add("amount", "amount");
                        //sbc.ColumnMappings.Add("qty", "openItem");
                        //sbc.ColumnMappings.Add("Product_Code", "Product_Code");
                        //sbc.ColumnMappings.Add("Product", "Product_Name");
                        //sbc.ColumnMappings.Add("Status", "Status");

                        //sbc.ColumnMappings.Add("Department_Id", "Department_Id");
                        //sbc.ColumnMappings.Add("Description", "Description");

                        sbc.DestinationTableName = _objpinvoiceproperty.InvoiceDetails.TableName;
                        sbc.WriteToServer(_objpinvoiceproperty.InvoiceDetails);

                    }
                    #endregion
                    #region Inventory Log and Inventory

                    for (int i = 0; i < _objpinvoiceproperty.InvoiceDetails.Rows.Count; i++)
                    {
                        cmdToExecute = new SqlCommand();
                        cmdToExecute.CommandText = "sp_InventoryMaster";
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.Connection = _mainConnection;


                        cmdToExecute.Parameters.Add(new SqlParameter("@productid", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, Convert.ToInt16(_objpinvoiceproperty.InvoiceDetails.Rows[i]["itemIdx"])));
                        cmdToExecute.Parameters.Add(new SqlParameter("@newqty", SqlDbType.Decimal, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(_objpinvoiceproperty.InvoiceDetails.Rows[i]["Qty"])));
                        cmdToExecute.Parameters.Add(new SqlParameter("@newrate", SqlDbType.Decimal, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(_objpinvoiceproperty.InvoiceDetails.Rows[i]["UnitPrice"])));
                        cmdToExecute.Parameters.Add(new SqlParameter("@wareHouseIdx", SqlDbType.Decimal, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, (_objpinvoiceproperty.WarerHouseID)));

                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();
                    }

                    cmdToExecute = new SqlCommand();
                    // cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandText = "dbo.[sp_ProcessPIForLogs]";
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.Connection = _mainConnection;
                    cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, piIdx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@WarehouseIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.WarerHouseID));
                    //cmdToExecute.Parameters.Add(new SqlParameter("@WarehouseIdx", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed,));
                    cmdToExecute.Transaction = this.Transaction;
                    _rowsAffected = cmdToExecute.ExecuteNonQuery();

                    #endregion
                    #region Account Master Entry
                    cmdToExecute.Parameters.Add(new SqlParameter("@glIdx", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.glIdx));

                    //int GLIDX = (Int32)cmdToExecute.Parameters["@glIdx"].Value;
                    //purchase entry for account gj same for all types
                    cmdToExecute = new SqlCommand();
                    // cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandText = "sp_InsertAccountGj";
                    cmdToExecute.Connection = _mainConnection;
                    cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                    cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                    cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 5));
                    cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.NetAmount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                    cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                    cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                    cmdToExecute.Transaction = this.Transaction;
                    _rowsAffected = cmdToExecute.ExecuteNonQuery();

                    //purchase end

                    //TAX entries
                    #region Account Gj Tax Entries
                    if (_objpinvoiceproperty.TaxData != null)
                    {
                        for (int i = 0; i < _objpinvoiceproperty.TaxData.Rows.Count; i++)
                        {

                            decimal taxpercntage = Convert.ToDecimal(_objpinvoiceproperty.TaxData.Rows[i]["TaxPercent"].ToString());
                            int taxid = Convert.ToInt32(_objpinvoiceproperty.TaxData.Rows[i]["Tax_Id"].ToString());
                            decimal taxamount = ((_objpinvoiceproperty.NetAmount / 100) * (taxpercntage));
                            cmdToExecute = new SqlCommand();
                            // cmdToExecute.CommandType = CommandType.StoredProcedure;
                            cmdToExecute.CommandType = CommandType.StoredProcedure;
                            cmdToExecute.CommandText = "sp_InsertAccountGj";
                            cmdToExecute.Connection = _mainConnection;
                            cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                            cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                            cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                            cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                            cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            //cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxid)); Yahan Tax ki coaIdx girna Chaye
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxid)); //sare tax k liye hardCodeValue hau
                            cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                            cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxamount));
                            cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                            cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                            cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                            cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                            cmdToExecute.Transaction = this.Transaction;
                            _rowsAffected = cmdToExecute.ExecuteNonQuery();
                        }
                    }
                    #endregion

                    #region on Payment
                    if (_objpinvoiceproperty.PaidAmount == 0)
                    {

                        ////cash entry bank
                        //cmdToExecute = new SqlCommand();
                        //// cmdToExecute.CommandType = CommandType.StoredProcedure;

                        //cmdToExecute.CommandType = CommandType.StoredProcedure;
                        //cmdToExecute.CommandText = "sp_InsertAccountGj";

                        //cmdToExecute.Connection = _mainConnection;
                        //cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        //cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        ////cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        //if (_objpinvoiceproperty.PaymentType == 1)
                        //{
                        //    //cash
                        //    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        //}
                        //if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        //{
                        //    //bank
                        //    // var data= _objpinvoicepropert
                        //    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        //}
                        //cmdToExecute.Transaction = this.Transaction;
                        //_rowsAffected = cmdToExecute.ExecuteNonQuery();


                        //acount payable
                        cmdToExecute = new SqlCommand();
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));
                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorCoaidx));
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();
                    }
                    #endregion

                    #region Full Payment

                    else if (_objpinvoiceproperty.BalanceAmount == 0)
                    {

                        //cash entry bank
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;

                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";

                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        if (_objpinvoiceproperty.PaymentType == 1)
                        {
                            //cash
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        }
                        if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        {
                            //bank
                            // var data= _objpinvoicepropert
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        }



                        cmdToExecute.Transaction = this.Transaction;
                        //this.StartTransaction();
                        // cmdToExecute.Transaction = this.Transaction;
                        // Execute query.
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();

                        //cash end


                    }


                    #endregion


                    #region partial Payment
                    else if ((_objpinvoiceproperty.PaidAmount != _objpinvoiceproperty.TotalAmount) && _objpinvoiceproperty.PaidAmount != 0)
                    {

                        //cash entry bank
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;

                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";

                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                        if (_objpinvoiceproperty.PaymentType == 1)
                        {
                            //cash
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
                        }
                        if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
                        {
                            //bank
                            // var data= _objpinvoicepropert
                            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
                        }
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();


                        //acount payable
                        cmdToExecute = new SqlCommand();
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));
                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorCoaidx));
                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();

                        #endregion





                        #endregion
                    }
                    else
                    {
                        if (_errorCode != (int)LLBLError.AllOk)
                        {
                            // Throw error.
                            this.RollBack();
                            throw new Exception("Stored Procedure 'sp_TRANSACTION_MASTER_Insert' reported the ErrorCode: " + _errorCode);

                        }
                    }
                }

                
                this.Commit();
                return true;
            }
            catch (Exception ex)
            {
                this.RollBack();
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("TRANSACTION_MASTER::Insert::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    //// Close connection.
                    //_mainConnection.Close();
                    CloseConnection();
                }
                cmdToExecute.Dispose();
            }
        }

        // Insert Direct Purchase
        //public  bool InsertDP()
        //{
        //    SqlCommand cmdToExecute = new SqlCommand();

        //    if (_objpinvoiceproperty.idx > 0)
        //    {
        //        //sp_PurchaseUpdate
        //        cmdToExecute.CommandText = "dbo.[sp_PurchaseUpdate]";
        //    }
        //    else
        //    {
        //        cmdToExecute.CommandText = "dbo.[sp_PInvoice_Insert]";
        //    }

        //    cmdToExecute.CommandType = CommandType.StoredProcedure;

        //    // Use base class' connection object
        //    cmdToExecute.Connection = _mainConnection;

        //    try
        //    {
        //        if (_objpinvoiceproperty.idx > 0)
        //        {
        //            //    cmdToExecute.Parameters.Add(new SqlParameter("@poNumber", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.poNumber));
        //            //    cmdToExecute.Parameters.Add(new SqlParameter("@vendorIdx", SqlDbType.Int, 50, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.vendorIdx));
        //            //    cmdToExecute.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar, 80, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.description));
        //            //    cmdToExecute.Parameters.Add(new SqlParameter("@totalamount", SqlDbType.Decimal, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.totalAmount));

        //            //    cmdToExecute.Parameters.Add(new SqlParameter("@creationdate", SqlDbType.DateTime, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objPOMasterProperty.creationDate));

        //            //    cmdToExecute.Parameters.Add(new SqlParameter("@createdbyuser", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.createdByUserIdx));
        //            //    cmdToExecute.Parameters.Add(new SqlParameter("@visible", SqlDbType.Int, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objPOMasterProperty.visible));
        //            //    cmdToExecute.Parameters.Add(new SqlParameter("@status", SqlDbType.Int, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objPOMasterProperty.status));


        //            //    cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.idx));
        //            //    cmdToExecute.Parameters.Add(new SqlParameter("@MRNIdx", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.MRNIdx));
        //        }
        //        else
        //        {
        //            cmdToExecute.Parameters.Add(new SqlParameter("@InvoiceNo", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.Decimal, 50, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.TotalAmount));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Decimal, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.NetAmount));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@createdBy", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.Status));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@PaymentType", SqlDbType.Int, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaymentType));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@IsPaid", SqlDbType.Bit, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.IsPaid));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@Description", SqlDbType.Text, 20, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.Description));

        //            cmdToExecute.Parameters.Add(new SqlParameter("@PaidAmount", SqlDbType.Decimal, 80, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@BalanceAmount", SqlDbType.Decimal, 9, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@Taxable", SqlDbType.Bit, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objpinvoiceproperty.Taxable));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@ParentDocID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.ParentDocID));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@VendorID", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));

        //            cmdToExecute.Parameters.Add(new SqlParameter("@TaxAmount", SqlDbType.Decimal, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.TaxAmount));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@Reference", SqlDbType.Text, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.Reference));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@BankId", SqlDbType.Int, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BankId));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@AccountChequeNo", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.AccountChequeNo));


        //            cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.idx));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@glIdx", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.glIdx));

        //            cmdToExecute.Parameters.Add(new SqlParameter("@IncometaxPercent", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@ITAmount", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@whSalesTaxAmount", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, null));

        //        }

        //        if (_mainConnectionIsCreatedLocal)
        //        {

        //            OpenConnection();
        //        }
        //        else
        //        {
        //            if (_mainConnectionProvider.IsTransactionPending)
        //            {
        //                cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
        //            }
        //        }

        //        this.StartTransaction();
        //        cmdToExecute.Transaction = this.Transaction;
        //        // Execute query.
        //        _rowsAffected = cmdToExecute.ExecuteNonQuery();

        //        //_iD = (Int32)cmdToExecute.Parameters["@iID"].Value;
        //        //_errorCode = cmdToExecute.Parameters["@ErrorCode"].Value;
        //        #region TAX DETAILS


        //        if (_objpinvoiceproperty.TaxData != null)
        //        {
        //            foreach (DataRow row in _objpinvoiceproperty.TaxData.Rows)
        //            {
        //                row["PID"] = cmdToExecute.Parameters["@ID"].Value.ToString();
        //            }
        //            _objpinvoiceproperty.TaxData.AcceptChanges();

        //            SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
        //            _objpinvoiceproperty.TaxData.TableName = "PI_Taxes";

        //            sbc.ColumnMappings.Clear();
        //            sbc.ColumnMappings.Add("Tax_Id", "Tax_Id");
        //            sbc.ColumnMappings.Add("PID", "PID");
        //            sbc.ColumnMappings.Add("TaxPercent", "taxPercent");
        //            //sbc.ColumnMappings.Add(2, 1);
        //            //sbc.ColumnMappings.Add("productTypeIdx", "productTypeIdx");
        //            //sbc.ColumnMappings.Add("itemIdx", "itemIdx");
        //            //sbc.ColumnMappings.Add("unitPrice", "unitPrice");
        //            //sbc.ColumnMappings.Add("qty", "qty");
        //            //sbc.ColumnMappings.Add("amount", "amount");
        //            //sbc.ColumnMappings.Add("qty", "openItem");
        //            //sbc.ColumnMappings.Add("Product_Code", "Product_Code");
        //            //sbc.ColumnMappings.Add("Product", "Product_Name");
        //            //sbc.ColumnMappings.Add("Status", "Status");

        //            //sbc.ColumnMappings.Add("Department_Id", "Department_Id");
        //            //sbc.ColumnMappings.Add("Description", "Description");

        //            sbc.DestinationTableName = _objpinvoiceproperty.TaxData.TableName;
        //            sbc.WriteToServer(_objpinvoiceproperty.TaxData);

        //        }
        //        if (_objpinvoiceproperty.InvoiceDetails != null)
        //        {
        //            foreach (DataRow row in _objpinvoiceproperty.InvoiceDetails.Rows)
        //                row["PIIdx"] = cmdToExecute.Parameters["@ID"].Value.ToString();

        //            _objpinvoiceproperty.InvoiceDetails.AcceptChanges();

        //            SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
        //            _objpinvoiceproperty.InvoiceDetails.TableName = "PI_Details";

        //            sbc.ColumnMappings.Clear();
        //            sbc.ColumnMappings.Add("ItemIdx", "ItemIdx");
        //            sbc.ColumnMappings.Add("Qty", "Qty");
        //            sbc.ColumnMappings.Add("PIIdx", "PIIdx");
        //            sbc.ColumnMappings.Add("UnitPrice", "UnitPrice");
        //            sbc.ColumnMappings.Add("TotalAmount", "TotalAmount");
        //            //sbc.ColumnMappings.Add("Status", "Status");
        //            //sbc.ColumnMappings.Add(2, 1);
        //            //sbc.ColumnMappings.Add("productTypeIdx", "productTypeIdx");
        //            //sbc.ColumnMappings.Add("itemIdx", "itemIdx");
        //            //sbc.ColumnMappings.Add("unitPrice", "unitPrice");
        //            //sbc.ColumnMappings.Add("qty", "qty");
        //            //sbc.ColumnMappings.Add("amount", "amount");
        //            //sbc.ColumnMappings.Add("qty", "openItem");
        //            //sbc.ColumnMappings.Add("Product_Code", "Product_Code");
        //            //sbc.ColumnMappings.Add("Product", "Product_Name");
        //            //sbc.ColumnMappings.Add("Status", "Status");

        //            //sbc.ColumnMappings.Add("Department_Id", "Department_Id");
        //            //sbc.ColumnMappings.Add("Description", "Description");

        //            sbc.DestinationTableName = _objpinvoiceproperty.InvoiceDetails.TableName;
        //            sbc.WriteToServer(_objpinvoiceproperty.InvoiceDetails);

        //            //inventry
        //            for (int i = 0; i < _objpinvoiceproperty.InvoiceDetails.Rows.Count; i++)
        //            {
        //                cmdToExecute = new SqlCommand();
        //                cmdToExecute.CommandText = "sp_InventoryMaster";
        //                cmdToExecute.CommandType = CommandType.StoredProcedure;
        //                cmdToExecute.Connection = _mainConnection;


        //                cmdToExecute.Parameters.Add(new SqlParameter("@productid", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, Convert.ToInt16(_objpinvoiceproperty.InvoiceDetails.Rows[i]["ItemIdx"])));
        //                cmdToExecute.Parameters.Add(new SqlParameter("@newqty", SqlDbType.Decimal, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(_objpinvoiceproperty.InvoiceDetails.Rows[i]["Qty"])));
        //                cmdToExecute.Parameters.Add(new SqlParameter("@newrate", SqlDbType.Decimal, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, Convert.ToDecimal(_objpinvoiceproperty.InvoiceDetails.Rows[i]["UnitPrice"])));

        //                cmdToExecute.Transaction = this.Transaction;
        //                _rowsAffected = cmdToExecute.ExecuteNonQuery();
        //            }

        //        }
        //        #endregion

        //        #region Account Master Entry
        //        cmdToExecute.Parameters.Add(new SqlParameter("@glIdx", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.glIdx));

        //        int GLIDX = (Int32)cmdToExecute.Parameters["@glIdx"].Value;
        //        //purchase entry for account gj same for all types
        //        cmdToExecute = new SqlCommand();
        //        // cmdToExecute.CommandType = CommandType.StoredProcedure;
        //        cmdToExecute.CommandType = CommandType.StoredProcedure;
        //        cmdToExecute.CommandText = "sp_InsertAccountGj";
        //        cmdToExecute.Connection = _mainConnection;
        //        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
        //        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

        //        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

        //        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
        //        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 5));
        //        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
        //        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.NetAmount));
        //        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
        //        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
        //        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

        //        cmdToExecute.Transaction = this.Transaction;
        //        _rowsAffected = cmdToExecute.ExecuteNonQuery();

        //        //purchase end

        //        //TAX entries
        //        #region Account Gj Tax Entries
        //        if (_objpinvoiceproperty.TaxData != null)
        //        {
        //            for (int i = 0; i < _objpinvoiceproperty.TaxData.Rows.Count; i++)
        //            {

        //                decimal taxpercntage = Convert.ToDecimal(_objpinvoiceproperty.TaxData.Rows[i]["TaxPercent"].ToString());
        //                int taxid = Convert.ToInt32(_objpinvoiceproperty.TaxData.Rows[i]["Tax_Id"].ToString());
        //                decimal taxamount = ((_objpinvoiceproperty.NetAmount / 100) * (taxpercntage));
        //                cmdToExecute = new SqlCommand();
        //                // cmdToExecute.CommandType = CommandType.StoredProcedure;
        //                cmdToExecute.CommandType = CommandType.StoredProcedure;
        //                cmdToExecute.CommandText = "sp_InsertAccountGj";
        //                cmdToExecute.Connection = _mainConnection;
        //                cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
        //                cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 111));

        //                cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

        //                cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
        //                cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //                cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //                //cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxid)); Bhae Taxes ki coaIdx Banani Hai 
        //                cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 12122));
        //                cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
        //                cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxamount));
        //                cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
        //                cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
        //                cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //                cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

        //                cmdToExecute.Transaction = this.Transaction;
        //                _rowsAffected = cmdToExecute.ExecuteNonQuery();
        //            }
        //        }
        //        #endregion

        //        #region Full Payment

        //        if (_objpinvoiceproperty.BalanceAmount == 0)
        //        {

        //            //cash entry bank
        //            cmdToExecute = new SqlCommand();
        //            // cmdToExecute.CommandType = CommandType.StoredProcedure;

        //            cmdToExecute.CommandType = CommandType.StoredProcedure;
        //            cmdToExecute.CommandText = "sp_InsertAccountGj";

        //            cmdToExecute.Connection = _mainConnection;
        //            cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

        //            cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

        //            cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

        //            cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
        //            //cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));

        //            if (_objpinvoiceproperty.PaymentType == 1)
        //            {
        //                //cash
        //                cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
        //            }
        //            if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
        //            {
        //                //bank
        //                // var data= _objpinvoicepropert
        //                cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
        //            }



        //            cmdToExecute.Transaction = this.Transaction;
        //            //this.StartTransaction();
        //            // cmdToExecute.Transaction = this.Transaction;
        //            // Execute query.
        //            _rowsAffected = cmdToExecute.ExecuteNonQuery();

        //            //cash end


        //        }


        //        #endregion


        //        #region partial Payment
        //        else if (_objpinvoiceproperty.PaidAmount != _objpinvoiceproperty.TotalAmount && _objpinvoiceproperty.PaidAmount != 0)
        //        {

        //            //cash entry bank
        //            cmdToExecute = new SqlCommand();
        //            // cmdToExecute.CommandType = CommandType.StoredProcedure;

        //            cmdToExecute.CommandType = CommandType.StoredProcedure;
        //            cmdToExecute.CommandText = "sp_InsertAccountGj";

        //            cmdToExecute.Connection = _mainConnection;
        //            cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));

        //            cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));

        //            cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

        //            cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.PaidAmount));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            //cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
        //            if (_objpinvoiceproperty.PaymentType == 1)
        //            {
        //                //cash
        //                cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 7));
        //            }
        //            if (_objpinvoiceproperty.PaymentType == 2 || _objpinvoiceproperty.PaymentType == 3)
        //            {
        //                //bank
        //                // var data= _objpinvoicepropert
        //                cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
        //            }
        //            cmdToExecute.Transaction = this.Transaction;
        //            _rowsAffected = cmdToExecute.ExecuteNonQuery();


        //            //acount payable
        //            cmdToExecute = new SqlCommand();
        //            cmdToExecute.CommandType = CommandType.StoredProcedure;
        //            cmdToExecute.CommandText = "sp_InsertAccountGj";
        //            cmdToExecute.Connection = _mainConnection;
        //            cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorCoaidx));
        //            cmdToExecute.Transaction = this.Transaction;
        //            _rowsAffected = cmdToExecute.ExecuteNonQuery();
        //        }
        //        #endregion

        //        #region No Pay

        //        else if (_objpinvoiceproperty.PaidAmount == 0)
        //        {
        //            //cash entry bank
        //            cmdToExecute = new SqlCommand();
        //            // cmdToExecute.CommandType = CommandType.StoredProcedure;
        //            cmdToExecute.CommandType = CommandType.StoredProcedure;
        //            cmdToExecute.CommandText = "sp_InsertAccountGj";
        //            cmdToExecute.Connection = _mainConnection;
        //            cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 1));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedBy));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.VendorID));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.InvoiceNo));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.BalanceAmount));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CreatedDate));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
        //            cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.CoaIDx));
        //            cmdToExecute.Transaction = this.Transaction;
        //            _rowsAffected = cmdToExecute.ExecuteNonQuery();
        //        }
        //        #endregion

        //        #endregion
        //        else
        //        {
        //            if (_errorCode != (int)LLBLError.AllOk)
        //            {
        //                // Throw error.
        //                this.RollBack();
        //                throw new Exception("Stored Procedure 'sp_TRANSACTION_MASTER_Insert' reported the ErrorCode: " + _errorCode);

        //            }

        //        }


        //        this.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        this.RollBack();
        //        // some error occured. Bubble it to caller and encapsulate Exception object
        //        throw new Exception("TRANSACTION_MASTER::Insert::Error occured.", ex);
        //    }
        //    finally
        //    {
        //        if (_mainConnectionIsCreatedLocal)
        //        {
        //            //// Close connection.
        //            //_mainConnection.Close();
        //            CloseConnection();
        //        }
        //        cmdToExecute.Dispose();
        //    }
        //}


        public  DataTable SelectAllByVendors(int id)
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "select * from P_Invoice where VendorID=@vendorIdx and isPaid=0 and BalanceAmount>0 ";
            //cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("Banks");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@vendorIdx", SqlDbType.Int, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, id));
               

                if (_mainConnectionIsCreatedLocal)
                {
                    // Open connection.
                    _mainConnection.Open();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }
                // Execute query.
                adapter.Fill(toReturn);
                //  _errorCode = (Int32)cmdToExecute.Parameters["@iErrorCode"].Value;

                //if (_errorCode != (int)LLBLError.AllOk)
                //{
                //    // Throw error.
                //    throw new Exception("Stored Procedure 'pr_PRODUCT_SETUP_SelectAll' reported the ErrorCode: " + _errorCode);
                //}

                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("PRODUCT_SETUP::SelectAll::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    // Close connection.
                    _mainConnection.Close();
                }
                cmdToExecute.Dispose();
                adapter.Dispose();
            }
        }


        public DataTable SelectAllByVendorID(int id)
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "select * from P_Invoice where VendorID=@vendorIdx  ";
            //cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("Banks");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@vendorIdx", SqlDbType.Int, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, id));


                if (_mainConnectionIsCreatedLocal)
                {
                    // Open connection.
                    _mainConnection.Open();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }
                // Execute query.
                adapter.Fill(toReturn);
                //  _errorCode = (Int32)cmdToExecute.Parameters["@iErrorCode"].Value;

                //if (_errorCode != (int)LLBLError.AllOk)
                //{
                //    // Throw error.
                //    throw new Exception("Stored Procedure 'pr_PRODUCT_SETUP_SelectAll' reported the ErrorCode: " + _errorCode);
                //}

                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("PRODUCT_SETUP::SelectAll::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    // Close connection.
                    _mainConnection.Close();
                }
                cmdToExecute.Dispose();
                adapter.Dispose();
            }
        }

        public DataSet SelectAllPIDetailsDataById(int id)
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = @"select pd.*,po.InvoiceNo,po.TotalAmount as MasterTotalAmount, po.NetAmount as MasterNetAmount
,po.isPaid,po.paymentType,po.status,po.BalanceAmount,po.PaidAmount,po.Description,po.Taxable,po.ParentDocID,
po.VendorID,po.BankId
from P_Invoice po
left join PI_Details pd on pd.PIIdx = po.idx
where po.idx =@idx
select * from PI_Taxes where PID =@idx";
            //cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("Banks");
            DataSet DS = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@idx", SqlDbType.Int, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, id));


                if (_mainConnectionIsCreatedLocal)
                {
                    // Open connection.
                    _mainConnection.Open();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }
                // Execute query.
                adapter.Fill(DS);
                //  _errorCode = (Int32)cmdToExecute.Parameters["@iErrorCode"].Value;

                //if (_errorCode != (int)LLBLError.AllOk)
                //{
                //    // Throw error.
                //    throw new Exception("Stored Procedure 'pr_PRODUCT_SETUP_SelectAll' reported the ErrorCode: " + _errorCode);
                //}

                return DS;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("PRODUCT_SETUP::SelectAll::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    // Close connection.
                    _mainConnection.Close();
                }
                cmdToExecute.Dispose();
                adapter.Dispose();
            }
        }
        public override DataTable SelectAll()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_GetALLPI]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("Banks");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                //cmdToExecute.Parameters.Add(new SqlParameter("@ProductCode", SqlDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Product_Code));
                // cmdToExecute.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.VarChar, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Product_Name));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Product_Parent_Code", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Product_Parent_Code));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Product_Conversion", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Product_Conversion));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Product_Level", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Product_Level));
                //   cmdToExecute.Parameters.Add(new SqlParameter("@Narration", SqlDbType.NVarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Narration));
                //   cmdToExecute.Parameters.Add(new SqlParameter("@Product_Type_ID", SqlDbType.Int, 5, ParameterDirection.Input, false, 1, 1, "", DataRowVersion.Proposed, ObjProductSetupProperty.Product_Type));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Packing_Unit_ID", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Packing_Unit_ID));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Small_Unit", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Small_Unit));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Small_Weight", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Small_Weight));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Small_UOM_Length", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Small_UOM_Length));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Small_UOM_Width", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Small_UOM_Width));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Small_UOM_Height", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Small_UOM_Height));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Dozen_Unit", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Dozen_Unit));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Dozen_Weight", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Dozen_Weight));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Dozen_UOM_Length", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Dozen_UOM_Length));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Dozen_UOM_Width", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Dozen_UOM_Width));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Dozen_UOM_Height", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Dozen_UOM_Height));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Conversion_Unit", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Conversion_Unit));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Conversion_Weight", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Conversion_Weight));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Conversion_UOM_Length", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Conversion_UOM_Length));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Conversion_UOM_Width", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Conversion_UOM_Width));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Conversion_UOM_Height", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Conversion_UOM_Height));
                // cmdToExecute.Parameters.Add(new SqlParameter("@PriceApplyOn", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.PriceApplyOn));
                // cmdToExecute.Parameters.Add(new SqlParameter("@SKU_Previous_Code", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.SKU_Previous_Code));
                // cmdToExecute.Parameters.Add(new SqlParameter("@GST", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.GST));
                // cmdToExecute.Parameters.Add(new SqlParameter("@GST_Free_SKU", SqlDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.GST_Free_SKU));
                // cmdToExecute.Parameters.Add(new SqlParameter("@GST_Schedule_ID", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.GST_Schedule_ID));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Sale_Price", SqlDbType.Decimal, 9, ParameterDirection.Input, false, 18, 1, "", DataRowVersion.Proposed, ObjProductSetupProperty.Sale_Price));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Purchase_Price", SqlDbType.Decimal, 9, ParameterDirection.Input, false, 18, 1, "", DataRowVersion.Proposed, ObjProductSetupProperty.Purchase_Price));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Sale_Days", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Sale_Days));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Stop_Days", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Stop_Days));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Qty_Limit", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Qty_Limit));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Value_Limit", SqlDbType.Decimal, 9, ParameterDirection.Input, false, 18, 1, "", DataRowVersion.Proposed, ObjProductSetupProperty.Value_Limit));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Is_Active", SqlDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Is_Active));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Defined_Date", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Defined_Date));
                // cmdToExecute.Parameters.Add(new SqlParameter("@Defined_By", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Defined_By));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Is_Final_Product", SqlDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Is_Final_Product));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.Status));
                //cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.ID));
                //// cmdToExecute.Parameters.Add(new SqlParameter("@iErrorCode", SqlDbType.Int, 4, ParameterDirection.Output, false, 10, 0, "", DataRowVersion.Proposed, _errorCode));

                //cmdToExecute.Parameters.Add(new SqlParameter("@PageNum", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.PageNum));
                //cmdToExecute.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.PageSize));
                //cmdToExecute.Parameters.Add(new SqlParameter("@sortColumn", SqlDbType.VarChar, 100, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.SortColumn));
                //cmdToExecute.Parameters.Add(new SqlParameter("@TotalRowsNum", SqlDbType.Int, 4, ParameterDirection.Output, false, 10, 0, "", DataRowVersion.Proposed, ObjProductSetupProperty.TotalRowsNum));

                if (_mainConnectionIsCreatedLocal)
                {
                    // Open connection.
                    _mainConnection.Open();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }
                // Execute query.
                adapter.Fill(toReturn);
                //  _errorCode = (Int32)cmdToExecute.Parameters["@iErrorCode"].Value;

                //if (_errorCode != (int)LLBLError.AllOk)
                //{
                //    // Throw error.
                //    throw new Exception("Stored Procedure 'pr_PRODUCT_SETUP_SelectAll' reported the ErrorCode: " + _errorCode);
                //}

                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("PRODUCT_SETUP::SelectAll::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    // Close connection.
                    _mainConnection.Close();
                }
                cmdToExecute.Dispose();
                adapter.Dispose();
            }
        }
        public override DataTable SelectOne()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_SelctPIByid]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("Bank Setup");
            DataSet DS = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
             cmdToExecute.Parameters.Add(new SqlParameter("@poid", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.idx));

                if (_mainConnectionIsCreatedLocal)
                {
                    // Open connection.
                    _mainConnection.Open();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }

                // Execute query.
                adapter.Fill(toReturn);


                if (toReturn.Rows.Count > 0)
                {
                    //ObjDepartmentProperty.Department_Id = (Int32)toReturn.Rows[0]["ID"];
                    //ObjDepartmentProperty.DepartmentName = (string)toReturn.Rows[0]["DepartmentName"];
                    //objBankProperty.idx = Convert.ToInt32(toReturn.Rows[0]["idx"]);// == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["idx"]);
                    //objBankProperty.bankName = (toReturn.Rows[0]["bankName"].ToString());// == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["bankName"]).ToString();
                    //objBankProperty.creationDate = Convert.ToDateTime((toReturn.Rows[0]["creationDate"]));// == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["creationDate"]));

                    //objBankProperty.visible = (int)toReturn.Rows[0]["visible"];
                    //objBankProperty.createdByUserIdx = (int)toReturn.Rows[0]["createdByUserIdx"];

                    // ObjDepartmentProperty.ISActive = (bool)toReturn.Rows[0]["IsActive"];
                    // //toReturn.Rows[0]["Product_Form_ID"] == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["Product_Form_ID"];
                    // ObjDepartmentProperty.UserId = (int)toReturn.Rows[0]["UserID"];
                    //string a= (toReturn.Rows[0]["bankName"] == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["bankName"]).ToString();
                    // ObjDepartmentProperty.CreatedDate = (DateTime)toReturn.Rows[0]["DateCreated"];
                    //   ObjDepartmentProperty.CustomerAddress = (string)toReturn.Rows[0]["Address"];
                    //  toReturn.Rows[0]["Product_Parent_Code"] == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["Product_Parent_Code"];

                }
                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("PRODUCT_SETUP::SelectOne::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    // Close connection.
                    _mainConnection.Close();
                }
                cmdToExecute.Dispose();
                adapter.Dispose();
            }
        }

        public DataSet SelectByID()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_SelctPIByid]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("Bank Setup");
            DataSet DS = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 4, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _objpinvoiceproperty.idx));

                if (_mainConnectionIsCreatedLocal)
                {
                    // Open connection.
                    _mainConnection.Open();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }

                // Execute query.
                adapter.Fill(DS);


                if (toReturn.Rows.Count > 0)
                {
                    //ObjDepartmentProperty.Department_Id = (Int32)toReturn.Rows[0]["ID"];
                    //ObjDepartmentProperty.DepartmentName = (string)toReturn.Rows[0]["DepartmentName"];
                    //objBankProperty.idx = Convert.ToInt32(toReturn.Rows[0]["idx"]);// == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["idx"]);
                    //objBankProperty.bankName = (toReturn.Rows[0]["bankName"].ToString());// == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["bankName"]).ToString();
                    //objBankProperty.creationDate = Convert.ToDateTime((toReturn.Rows[0]["creationDate"]));// == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["creationDate"]));

                    //objBankProperty.visible = (int)toReturn.Rows[0]["visible"];
                    //objBankProperty.createdByUserIdx = (int)toReturn.Rows[0]["createdByUserIdx"];

                    // ObjDepartmentProperty.ISActive = (bool)toReturn.Rows[0]["IsActive"];
                    // //toReturn.Rows[0]["Product_Form_ID"] == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["Product_Form_ID"];
                    // ObjDepartmentProperty.UserId = (int)toReturn.Rows[0]["UserID"];
                    //string a= (toReturn.Rows[0]["bankName"] == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["bankName"]).ToString();
                    // ObjDepartmentProperty.CreatedDate = (DateTime)toReturn.Rows[0]["DateCreated"];
                    //   ObjDepartmentProperty.CustomerAddress = (string)toReturn.Rows[0]["Address"];
                    //  toReturn.Rows[0]["Product_Parent_Code"] == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["Product_Parent_Code"];

                }
                return DS;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("PRODUCT_SETUP::SelectOne::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    // Close connection.
                    _mainConnection.Close();
                }
                cmdToExecute.Dispose();
                adapter.Dispose();
            }
        }
        //public string GeneratePO()
        //{
        //    SqlCommand cmdToExecute = new SqlCommand();
        //    cmdToExecute.CommandText = "dbo.[sp_GeneratePONumber]";
        //    cmdToExecute.CommandType = CommandType.StoredProcedure;

        //    // Use base class' connection object
        //    cmdToExecute.Connection = _mainConnection;

        //    try
        //    {
        //        //cmdToExecute.Parameters.Add(new SqlParameter("@tablename", SqlDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ObjDepartmentProperty.TableName));
        //        //cmdToExecute.Parameters.Add(new SqlParameter("@coulmn", SqlDbType.VarChar, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed,"'"+ ObjDepartmentProperty.ColumnName+"'"));
        //        cmdToExecute.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.createdByUserIdx));
        //       // cmdToExecute.Parameters.Add(new SqlParameter("@PoNumber", SqlDbType.NVarChar, 32, ParameterDirection.Output, true, 10, 0, "", DataRowVersion.Proposed, _objPOMasterProperty.poNumber));
        //        if (_mainConnectionIsCreatedLocal)
        //        {
        //            // Open connection.
        //            _mainConnection.Open();
        //        }
        //        else
        //        {
        //            if (_mainConnectionProvider.IsTransactionPending)
        //            {
        //                cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
        //            }
        //        }
        //        // Execute query.
        //        //  _errorCode = (Int32)cmdToExecute.Parameters["@iErrorCode"].Value;

        //        //if (_errorCode != (int)LLBLError.AllOk)
        //        //{
        //        //    // Throw error.
        //        //    throw new Exception("Stored Procedure 'pr_PRODUCT_SETUP_SelectAll' reported the ErrorCode: " + _errorCode);
        //        //}

        //        //_rowsAffected = Convert.ToInt32(cmdToExecute.ExecuteScalar());
        //        return cmdToExecute.Parameters["@PoNumber"].Value.ToString(); //_rowsAffected;
        //    }
        //    catch (Exception ex)
        //    {
        //        // some error occured. Bubble it to caller and encapsulate Exception object
        //        throw new Exception("PRODUCT_SETUP::SelectAll::Error occured.", ex);
        //    }
        //    finally
        //    {
        //        if (_mainConnectionIsCreatedLocal)
        //        {
        //            // Close connection.
        //            _mainConnection.Close();
        //        }
        //        cmdToExecute.Dispose();
        //    }

        //}
        public DataTable SelectPIBYDate(LP_Voucher_ViewModel objVCHR)
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_SelctPIbyDate]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("Bank Setup");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@vendorid", SqlDbType.Int, 40, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objVCHR.vendor_id));
                cmdToExecute.Parameters.Add(new SqlParameter("@datefrom", SqlDbType.DateTime, 40, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objVCHR.From_Date));
                cmdToExecute.Parameters.Add(new SqlParameter("@dateto", SqlDbType.DateTime, 40, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objVCHR.To_Date));


                if (_mainConnectionIsCreatedLocal)
                {
                    // Open connection.
                    _mainConnection.Open();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }

                // Execute query.
                adapter.Fill(toReturn);


                if (toReturn.Rows.Count > 0)
                {
                    //ObjDepartmentProperty.Department_Id = (Int32)toReturn.Rows[0]["ID"];
                    //ObjDepartmentProperty.DepartmentName = (string)toReturn.Rows[0]["DepartmentName"];
                    //objBankProperty.idx = Convert.ToInt32(toReturn.Rows[0]["idx"]);// == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["idx"]);
                    //objBankProperty.bankName = (toReturn.Rows[0]["bankName"].ToString());// == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["bankName"]).ToString();
                    //objBankProperty.creationDate = Convert.ToDateTime((toReturn.Rows[0]["creationDate"]));// == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["creationDate"]));

                    //objBankProperty.visible = (int)toReturn.Rows[0]["visible"];
                    //objBankProperty.createdByUserIdx = (int)toReturn.Rows[0]["createdByUserIdx"];

                    // ObjDepartmentProperty.ISActive = (bool)toReturn.Rows[0]["IsActive"];
                    // //toReturn.Rows[0]["Product_Form_ID"] == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["Product_Form_ID"];
                    // ObjDepartmentProperty.UserId = (int)toReturn.Rows[0]["UserID"];
                    //string a= (toReturn.Rows[0]["bankName"] == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["bankName"]).ToString();
                    // ObjDepartmentProperty.CreatedDate = (DateTime)toReturn.Rows[0]["DateCreated"];
                    //   ObjDepartmentProperty.CustomerAddress = (string)toReturn.Rows[0]["Address"];
                    //  toReturn.Rows[0]["Product_Parent_Code"] == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["Product_Parent_Code"];

                }
                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("PRODUCT_SETUP::SelectOne::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    // Close connection.
                    _mainConnection.Close();
                }
                cmdToExecute.Dispose();
                adapter.Dispose();
            }
        }
        public DataTable GeneratePINo(LP_GenerateTransNumber_Property objTranNo)
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_GenerateTransNo]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("Po");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@tablename", SqlDbType.VarChar, 40, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objTranNo.TableName));
                cmdToExecute.Parameters.Add(new SqlParameter("@identityfieldname", SqlDbType.NVarChar, 40, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objTranNo.Identityfieldname));
                cmdToExecute.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 40, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objTranNo.userid));

                if (_mainConnectionIsCreatedLocal)
                {
                    // Open connection.
                    _mainConnection.Open();
                }
                else
                {
                    if (_mainConnectionProvider.IsTransactionPending)
                    {
                        cmdToExecute.Transaction = _mainConnectionProvider.CurrentTransaction;
                    }
                }

                // Execute query.
                adapter.Fill(toReturn);


                if (toReturn.Rows.Count > 0)
                {
                    //ObjDepartmentProperty.Department_Id = (Int32)toReturn.Rows[0]["ID"];
                    //ObjDepartmentProperty.DepartmentName = (string)toReturn.Rows[0]["DepartmentName"];
                    //objBankProperty.idx = Convert.ToInt32(toReturn.Rows[0]["idx"]);// == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["idx"]);
                    //objBankProperty.bankName = (toReturn.Rows[0]["bankName"].ToString());// == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["bankName"]).ToString();
                    //objBankProperty.creationDate = Convert.ToDateTime((toReturn.Rows[0]["creationDate"]));// == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["creationDate"]));

                    //objBankProperty.visible = (int)toReturn.Rows[0]["visible"];
                    //objBankProperty.createdByUserIdx = (int)toReturn.Rows[0]["createdByUserIdx"];

                    // ObjDepartmentProperty.ISActive = (bool)toReturn.Rows[0]["IsActive"];
                    // //toReturn.Rows[0]["Product_Form_ID"] == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["Product_Form_ID"];
                    // ObjDepartmentProperty.UserId = (int)toReturn.Rows[0]["UserID"];
                    //string a= (toReturn.Rows[0]["bankName"] == System.DBNull.Value ? SqlString.Null : (string)toReturn.Rows[0]["bankName"]).ToString();
                    // ObjDepartmentProperty.CreatedDate = (DateTime)toReturn.Rows[0]["DateCreated"];
                    //   ObjDepartmentProperty.CustomerAddress = (string)toReturn.Rows[0]["Address"];
                    //  toReturn.Rows[0]["Product_Parent_Code"] == System.DBNull.Value ? SqlInt32.Null : (Int32)toReturn.Rows[0]["Product_Parent_Code"];

                }
                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("PRODUCT_SETUP::SelectOne::Error occured.", ex);
            }
            finally
            {
                if (_mainConnectionIsCreatedLocal)
                {
                    // Close connection.
                    _mainConnection.Close();
                }
                cmdToExecute.Dispose();
                adapter.Dispose();
            }
        }
    }
}
