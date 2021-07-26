using SNDDAL;
using SSS.Property.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SSS.DAL.Transactions
{
    public  class LP_PIReturn_DAL : DBInteractionBase
    {
        private LP_PIReturn_Property _objMRMasterProperty;
        private LP_PIReturn_Details_Property _objMRDetailProperty;
        public LP_PIReturn_DAL(LP_PIReturn_Property objMRMasterProperty)
        {
            _objMRMasterProperty = objMRMasterProperty;
        }
        public LP_PIReturn_DAL(LP_PIReturn_Details_Property objMRDetailProperty)
        {
            _objMRDetailProperty = objMRDetailProperty;
        }

        public override bool Insert()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            //cmdToExecute.CommandText = "dbo.[sp_MRNInsert]";
            //if (_objMRMasterProperty.idx > 0)
            //{
                cmdToExecute.CommandText = @" insert into PIReturn(piIdx,invoiceNo,returnAmount,createdBy,creationDate,visible) VALUES
 (@piIdx,@invoiceNo,@returnAmount,@createdBy,@creationDate,@visible)
select @ID =  Ident_Current('accountMasterGL')  
 ";
            //}
            //else
            //{
            //    cmdToExecute.CommandText = "dbo.[sp_MRNInsert]";
            //}
            //cmdToExecute.CommandType = CommandType.StoredProcedure;

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@piIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.piIdx));

                cmdToExecute.Parameters.Add(new SqlParameter("@invoiceNo", SqlDbType.NVarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.invoiceNo));

                cmdToExecute.Parameters.Add(new SqlParameter("@returnAmount", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.returnAmount));

                cmdToExecute.Parameters.Add(new SqlParameter("@createdBy", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.createdBy));

                cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.DateTime, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, DateTime.Now));
                cmdToExecute.Parameters.Add(new SqlParameter("@visible", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 1));
                cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 500, ParameterDirection.Output, true, 0, 0, "", DataRowVersion.Proposed, 1));



                if (_mainConnectionIsCreatedLocal)
                {
                    // Open connection.
                    //   _mainConnection.Open();
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
                // _iD = (Int32)cmdToExecute.Parameters["@iID"].Value;
                //_errorCode = cmdToExecute.Parameters["@ErrorCode"].Value;

                if (_objMRMasterProperty.DetailData != null)
                {
                    foreach (DataRow row in _objMRMasterProperty.DetailData.Rows)
                    {
                        row["returnIdx"] = cmdToExecute.Parameters["@ID"].Value.ToString(); //cmdToExecute.Parameters["@ID"].Value.ToString();
                        row["PIIdx"] = _objMRMasterProperty.piIdx.ToString() ;
                    }
                     
                    
                    //row["mrnIdx"] = _rowsAffected;
                    _objMRMasterProperty.DetailData.AcceptChanges();

                    SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
                    _objMRMasterProperty.DetailData.TableName = "PIReturn_Details";

                    sbc.ColumnMappings.Clear();
                    sbc.ColumnMappings.Add("returnIdx", "returnIdx");
                    //sbc.ColumnMappings.Add(2, 1);
                    //sbc.ColumnMappings.Add("productTypeIdx", "productTypeIdx");
                    sbc.ColumnMappings.Add("itemIdx", "itemIdx");
                    //sbc.ColumnMappings.Add("unitPrice", "unitPrice");
                    sbc.ColumnMappings.Add("returnQty", "returnQty");
                    sbc.ColumnMappings.Add("ReturnAmount", "ReturnAmount");
                    sbc.ColumnMappings.Add("qty", "qty");
                    sbc.ColumnMappings.Add("unitPrice", "unitPrice");
                    //sbc.ColumnMappings.Add("amount", "amount");
                    //sbc.ColumnMappings.Add("qty", "openItem");
                    //sbc.ColumnMappings.Add("Product_Code", "Product_Code");
                    //sbc.ColumnMappings.Add("Product", "Product_Name");
                    //sbc.ColumnMappings.Add("Status", "Status");

                    //sbc.ColumnMappings.Add("Department_Id", "Department_Id");
                    //sbc.ColumnMappings.Add("Description", "Description");

                    sbc.DestinationTableName = _objMRMasterProperty.DetailData.TableName;
                    sbc.WriteToServer(_objMRMasterProperty.DetailData);

                }

                if (_objMRMasterProperty.DetailData != null)
                {
                    foreach (DataRow row in _objMRMasterProperty.DetailData.Rows)
                    {
                        row["returnIdx"] = cmdToExecute.Parameters["@ID"].Value.ToString(); //cmdToExecute.Parameters["@ID"].Value.ToString();
                        row["PIIdx"] = "16";
                        row["returnQty"] = Convert.ToDecimal(row["returnQty"].ToString()) * (-1);
                        row["CreatedDate"] = DateTime.Now.ToString() ;
                    }


                    //row["mrnIdx"] = _rowsAffected;
                    _objMRMasterProperty.DetailData.AcceptChanges();

                    SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
                    _objMRMasterProperty.DetailData.TableName = "inventory_logs";

                    sbc.ColumnMappings.Clear();
                    sbc.ColumnMappings.Add("returnIdx", "MasterID");
                    sbc.ColumnMappings.Add("PIIdx", "TransactionTypeID");
                    //sbc.ColumnMappings.Add(2, 1);
                    //sbc.ColumnMappings.Add("productTypeIdx", "productTypeIdx");
                    sbc.ColumnMappings.Add("itemIdx", "productIdx");
                    //sbc.ColumnMappings.Add("unitPrice", "unitPrice");
                    sbc.ColumnMappings.Add("returnQty", "stock");
                    sbc.ColumnMappings.Add("ReturnAmount", "totalAmount");
                    sbc.ColumnMappings.Add("CreatedDate", "creationDate");
                    sbc.ColumnMappings.Add("unitPrice", "unitPrice");
                    //sbc.ColumnMappings.Add("amount", "amount");
                    //sbc.ColumnMappings.Add("qty", "openItem");
                    //sbc.ColumnMappings.Add("Product_Code", "Product_Code");
                    //sbc.ColumnMappings.Add("Product", "Product_Name");
                    //sbc.ColumnMappings.Add("Status", "Status");

                    //sbc.ColumnMappings.Add("Department_Id", "Department_Id");
                    //sbc.ColumnMappings.Add("Description", "Description");

                    sbc.DestinationTableName = _objMRMasterProperty.DetailData.TableName;
                    sbc.WriteToServer(_objMRMasterProperty.DetailData);

                }
                cmdToExecute = new SqlCommand();
               // cmdToExecute.CommandType = CommandType.StoredProcedure;
                

                cmdToExecute.CommandType = CommandType.StoredProcedure;
                cmdToExecute.CommandText = "sp_updatePurchaseAndAccountsAfterReturn";
                cmdToExecute.Connection = _mainConnection;
                cmdToExecute.Parameters.Add(new SqlParameter("@piIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.piIdx));

                cmdToExecute.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Decimal, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.UpdatedNetAmount));

                cmdToExecute.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.UpdatedTotalAmount));

                cmdToExecute.Parameters.Add(new SqlParameter("@PaidAmount", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.UpdatedPaidAmount));
                cmdToExecute.Parameters.Add(new SqlParameter("@TaxAmount", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.UpdatedTaxAmount));
                cmdToExecute.Parameters.Add(new SqlParameter("@BalanceAmount", SqlDbType.Decimal, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.UpdatedBalanceAmount));
                cmdToExecute.Parameters.Add(new SqlParameter("@amountTobeReceived", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.amountTobeReceived));
                cmdToExecute.Parameters.Add(new SqlParameter("@lastModificationDate", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, DateTime.Now.ToString("yyyy-MM-dd")));
                cmdToExecute.Parameters.Add(new SqlParameter("@lastModifiedByUserIdx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.createdBy));
                cmdToExecute.Parameters.Add(new SqlParameter("@isPaid", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.isPaid));
                cmdToExecute.Parameters.Add(new SqlParameter("@glIdx", SqlDbType.Int, 500, ParameterDirection.Output, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.glIdx));

                cmdToExecute.Transaction = this.Transaction;
                //this.StartTransaction();
                // cmdToExecute.Transaction = this.Transaction;
                // Execute query.
                _rowsAffected = cmdToExecute.ExecuteNonQuery();
                if (_objMRMasterProperty.UpdatedDetailData != null)
                {
                    foreach (DataRow row in _objMRMasterProperty.UpdatedDetailData.Rows)
                    {
                       /* row["returnIdx"] = _rowsAffected.ToString();*/ //cmdToExecute.Parameters["@ID"].Value.ToString();
                        row["PIIdx"] = _objMRMasterProperty.piIdx.ToString();
                    }


                    //row["mrnIdx"] = _rowsAffected; dikhao ?? kha gye janab? lalit? abay kha gya? column names verify kro destination table jo sql ma m
                    _objMRMasterProperty.UpdatedDetailData.AcceptChanges();

                    SqlBulkCopy sbc = new SqlBulkCopy(_mainConnection, SqlBulkCopyOptions.Default, this.Transaction);
                    _objMRMasterProperty.UpdatedDetailData.TableName = "PI_Details";

                    sbc.ColumnMappings.Clear();
                    sbc.ColumnMappings.Add("PIIdx", "PIIdx");
                    //sbc.ColumnMappings.Add(2, 1);
                    //sbc.ColumnMappings.Add("productTypeIdx", "productTypeIdx");
                    sbc.ColumnMappings.Add("ItemIdx", "ItemIdx");
                    //sbc.ColumnMappings.Add("unitPrice", "unitPrice");
                    sbc.ColumnMappings.Add("Qty", "Qty");
                    sbc.ColumnMappings.Add("UnitPrice", "UnitPrice");
                    sbc.ColumnMappings.Add("TotalAmount", "TotalAmount");
                    sbc.ColumnMappings.Add("Status", "Status");
               

                    sbc.DestinationTableName = _objMRMasterProperty.UpdatedDetailData.TableName;
                    sbc.WriteToServer(_objMRMasterProperty.UpdatedDetailData);

                }
                int GLIDX = (Int32)cmdToExecute.Parameters["@glIdx"].Value;
                //purchase entry for account gj same for all types
                cmdToExecute = new SqlCommand();
                // cmdToExecute.CommandType = CommandType.StoredProcedure;
                 cmdToExecute.CommandType = CommandType.StoredProcedure;
                cmdToExecute.CommandText = "sp_InsertAccountGj";
                cmdToExecute.Connection = _mainConnection;
                cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 16));

                cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.createdBy));

                cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.VendorID));
                cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 10));
                cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.invoiceNo));
                cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.returnAmount));
                cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, DateTime.Now));
                cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                cmdToExecute.Transaction = this.Transaction;
                _rowsAffected = cmdToExecute.ExecuteNonQuery();


                for (int i = 0; i < _objMRMasterProperty.taxData.Rows.Count; i++)
                {

                    decimal taxpercntage = Convert.ToDecimal(_objMRMasterProperty.taxData.Rows[i]["TaxPercent"].ToString());
                    int taxid = Convert.ToInt32(_objMRMasterProperty.taxData.Rows[i]["Tax_Id"].ToString());
                    decimal taxamount = ((_objMRMasterProperty.returnAmount / 100) * (taxpercntage));
                    cmdToExecute = new SqlCommand();
                    // cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandText = "sp_InsertAccountGj";
                    cmdToExecute.Connection = _mainConnection;
                    cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 111));

                    cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.createdBy));

                    cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.VendorID));
                    cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxid));
                    cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.invoiceNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                    cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, taxamount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, DateTime.Now));
                    cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                    cmdToExecute.Transaction = this.Transaction;
                    _rowsAffected = cmdToExecute.ExecuteNonQuery();
                }

                if (_objMRMasterProperty.returnAmountInclusiveTax <= _objMRMasterProperty.BalancAmount)
                {
                    //same Entry jaye gi vendorCoaIdx bas Debit hogi total inclusive returntax
                    if (_objMRMasterProperty.returnAmountInclusiveTax == _objMRMasterProperty.BalancAmount)
                    {
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 16));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.createdBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.coaVendorIdx));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.invoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.returnAmountInclusiveTax));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, DateTime.Now));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();
                    }
                    else
                    {
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 16));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.createdBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.coaVendorIdx));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.invoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.returnAmountInclusiveTax));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, DateTime.Now));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();

                        

                    }
                }
                else if (_objMRMasterProperty.returnAmountInclusiveTax > _objMRMasterProperty.BalancAmount)
                {
                    decimal returnPlusTax = _objMRMasterProperty.returnAmountInclusiveTax;
                    decimal previousBalance = _objMRMasterProperty.BalancAmount;
                    if (previousBalance > 0)
                    {
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 16));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.createdBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.coaVendorIdx));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.invoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, previousBalance));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, DateTime.Now));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();

                        //Vendor Refund Entry
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 16));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.createdBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 11));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.invoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.amountTobeReceived));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, DateTime.Now));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();

                    }
                    else
                    {
                        //balance waise he zero hoga to sirf refund hoga
                        //Vendor Refund Entry
                        cmdToExecute = new SqlCommand();
                        // cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandType = CommandType.StoredProcedure;
                        cmdToExecute.CommandText = "sp_InsertAccountGj";
                        cmdToExecute.Connection = _mainConnection;
                        cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                        cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 16));

                        cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.createdBy));

                        cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.VendorID));
                        cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 11));
                        cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.invoiceNo));
                        cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objMRMasterProperty.amountTobeReceived));
                        cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                        cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, DateTime.Now));
                        cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                        cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                        cmdToExecute.Transaction = this.Transaction;
                        _rowsAffected = cmdToExecute.ExecuteNonQuery();
                    }
                     
                }

         
                this.Commit();
                if (_errorCode != (int)LLBLError.AllOk)
                {
                    // Throw error.
                    this.RollBack();
                    throw new Exception("Stored Procedure 'sp_TRANSACTION_MASTER_Insert' reported the ErrorCode: " + _errorCode);

                }

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
    }
}
