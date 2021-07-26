using SNDDAL;
using SSS.Property.Setups;
using SSS.Property.Setups.Accounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SSS.DAL.Setups.Accounts
{
    public class LP_OpenningEntry_DAL : DBInteractionBase
    {
        private OpenigEntryVM objOpeningProperty;
        private AccountMasterGL objGL;
        public LP_OpenningEntry_DAL()
        {
        }
        public LP_OpenningEntry_DAL(OpenigEntryVM objOpeningEntryVM)
        {
            objOpeningProperty = objOpeningEntryVM;
        }
        public override bool Insert()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            if (objOpeningProperty.idx > 0)
            {
                //sp_PurchaseUpdate
                cmdToExecute.CommandText = "dbo.[sp_PurchaseUpdate]";
            }
            else
            {
                cmdToExecute.CommandText = "dbo.[sp_InsertDataOpening]";
            }

            cmdToExecute.CommandType = CommandType.StoredProcedure;

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {

                if (objOpeningProperty.idx > 0)
                {
                    cmdToExecute.Parameters.Add(new SqlParameter("@voucherNo", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.voucherNo));


                    cmdToExecute.Parameters.Add(new SqlParameter("@netAmount", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.Amount));

                    cmdToExecute.Parameters.Add(new SqlParameter("@createDate", SqlDbType.DateTime, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, objOpeningProperty.createDate));

                    cmdToExecute.Parameters.Add(new SqlParameter("@createdByUserIdx", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.createdByUserIdx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@visible", SqlDbType.Int, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, objOpeningProperty.visible));
                    cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.idx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@glIdx", SqlDbType.Int, 32, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.glIdx));


                }
                else
                {

                   
                        
         
                        
                        
                   cmdToExecute.Parameters.Add(new SqlParameter("@voucherNo", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.voucherNo));                  
                    cmdToExecute.Parameters.Add(new SqlParameter("@netAmount", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.Amount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@createDate", SqlDbType.DateTime, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, objOpeningProperty.createDate));
                    cmdToExecute.Parameters.Add(new SqlParameter("@createdByUserIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.createdByUserIdx));
                    //cmdToExecute.Parameters.Add(new SqlParameter("@visible", SqlDbType.Int, 4, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, objOpeningProperty.visible));



                    cmdToExecute.Parameters.Add(new SqlParameter("@voucherDate", SqlDbType.DateTime, 32, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.voucherDate));
                    //wareHouseIdx,salespersonIdx,shippingCost,discount,taxAount,paymentModeIdx,bankIdx,accorChequeNumber

                    cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 32, ParameterDirection.Output, true, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.idx));

                    cmdToExecute.Parameters.Add(new SqlParameter("@memo", SqlDbType.NVarChar, 500, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.memo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@openingType", SqlDbType.NVarChar, 500, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.openingType));
             


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
                // _iD = (Int32)cmdToExecute.Parameters["@iID"].Value;
                //_errorCode = cmdToExecute.Parameters["@ErrorCode"].Value;


                int GLIDX = (Int32)cmdToExecute.Parameters["@ID"].Value;

                if (objOpeningProperty.openingType == 1)
                {

                  
                    //purchase entry for account gj same for all types
                    cmdToExecute = new SqlCommand();
                    // cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandText = "sp_InsertAccountGj";
                    cmdToExecute.Connection = _mainConnection;
                    cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 11));

                    cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.createdByUserIdx));

                    cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0));
                    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.accountIdx));
                    cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.voucherNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Amount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                    cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.createDate));
                    cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                    cmdToExecute.Transaction = this.Transaction;
                    _rowsAffected = cmdToExecute.ExecuteNonQuery();


                    cmdToExecute = new SqlCommand();
                    // cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandText = "sp_InsertAccountGj";
                    cmdToExecute.Connection = _mainConnection;
                    cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 11));

                    cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.createdByUserIdx));

                    cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0));
                    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 28));//customer opening
                    cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.voucherNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                    cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Amount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.createDate));
                    cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                    cmdToExecute.Transaction = this.Transaction;
                    _rowsAffected = cmdToExecute.ExecuteNonQuery();
                }
                else
                {
                   
                    //purchase entry for account gj same for all types
                    cmdToExecute = new SqlCommand();
                    // cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandText = "sp_InsertAccountGj";
                    cmdToExecute.Connection = _mainConnection;
                    cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 12));

                    cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.createdByUserIdx));

                    cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0));
                    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 6));
                    cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.voucherNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                    cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Amount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.createDate));
                    cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                    cmdToExecute.Transaction = this.Transaction;
                    _rowsAffected = cmdToExecute.ExecuteNonQuery();


                    cmdToExecute = new SqlCommand();
                    // cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandType = CommandType.StoredProcedure;
                    cmdToExecute.CommandText = "sp_InsertAccountGj";
                    cmdToExecute.Connection = _mainConnection;
                    cmdToExecute.Parameters.Add(new SqlParameter("@GLIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, GLIDX));
                    cmdToExecute.Parameters.Add(new SqlParameter("@TransTypeIdx", SqlDbType.Int, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, 12));

                    cmdToExecute.Parameters.Add(new SqlParameter("@useridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.createdByUserIdx));

                    cmdToExecute.Parameters.Add(new SqlParameter("@vendoridx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@employeeidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@customeridx", SqlDbType.Int, 25, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0));
                    cmdToExecute.Parameters.Add(new SqlParameter("@coaidx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 6));
                    cmdToExecute.Parameters.Add(new SqlParameter("@invoiceidx", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.voucherNo));
                    cmdToExecute.Parameters.Add(new SqlParameter("@debit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, 0.00m));
                    cmdToExecute.Parameters.Add(new SqlParameter("@credit", SqlDbType.Decimal, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Amount));
                    cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.createDate));
                    cmdToExecute.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));
                    cmdToExecute.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.Date, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, null));

                    cmdToExecute.Transaction = this.Transaction;
                    _rowsAffected = cmdToExecute.ExecuteNonQuery();
                }

             
                


     
                if (_errorCode != (int)LLBLError.AllOk)
                {
                    // Throw error.
                    this.RollBack();
                    throw new Exception("Stored Procedure 'sp_TRANSACTION_MASTER_Insert' reSOrted the ErrorCode: " + _errorCode);

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
        



        public override bool Update()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_Update_childheadAccounts]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                //cmdToExecute.Parameters.Add(new SqlParameter("@idx", SqlDbType.Int, 100, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.idx));
                //cmdToExecute.Parameters.Add(new SqlParameter("@subHeadIdx", SqlDbType.Int, 100, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.subHeadIdx));
                //cmdToExecute.Parameters.Add(new SqlParameter("@childHeadName", SqlDbType.NVarChar, 100, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.childHeadName));
                //cmdToExecute.Parameters.Add(new SqlParameter("@lastModificationDate", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.lasModificationDate));
                //cmdToExecute.Parameters.Add(new SqlParameter("@lastModifiedByUserIdx", SqlDbType.Int, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.lastModifiedByUserIdx));

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
                    throw new Exception("Stored Procedure 'sp_update_fourthTier' reported the ErrorCode: " + _errorCode);
                }

                return true;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("fourthTier::Update::Error occured.", ex);
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


        /// <summary>
        /// Purpose: SelectAll method. This method will Select all rows from the table.
        /// </summary>
        /// <returns>DataTable object if succeeded, otherwise an Exception is thrown. </returns>
        /// <remarks>
        /// Properties set after a succesful call of this method: 
        /// <UL>
        ///		 <LI>ErrorCode</LI>
        /// </UL>
        /// </remarks>
        public override DataTable SelectAll()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            //cmdToExecute.CommandText = "dbo.[sp_fourthTier_SelectAll]";
            cmdToExecute.CommandText = @"select sh.subHeadName,ch.accountName as headName,Concat(us.firstName,'',us.lastName) as userName,co.* from ChartofAccountsChildHead co
inner join Users us on co.createdByUserIdx=us.idx 
inner join chartOFAccountsHead ch on ch.idx=co.headIdx
inner join ChartofAccountsSubHead sh on sh.idx=co.subHeadIdx
where co.visible=1 
order by idx desc";
            //cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("fourthTier");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;

            try
            {
                //cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.ID));
                //cmdToExecute.Parameters.Add(new SqlParameter("@fourthTier_Code", SqlDbType.VarChar, 20, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.fourthTier_Code));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Name));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Short_Description", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Short_Description));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Long_Description", SqlDbType.NVarChar, 1500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Long_Description));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar, 500, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Address));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Phone_Number1", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Phone_Number1));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Phone_Number2", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Phone_Number2));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Fax_Number", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Fax_Number));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Email));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Website", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Website));
                //cmdToExecute.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.Status));
                //cmdToExecute.Parameters.Add(new SqlParameter("@PageNum", SqlDbType.Int, 32, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.PageNum));
                //cmdToExecute.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int, 32, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.PageSize));
                //cmdToExecute.Parameters.Add(new SqlParameter("@sortColumn", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.SortColumn));
                //cmdToExecute.Parameters.Add(new SqlParameter("@TotalRowsNum", SqlDbType.Int, 32, ParameterDirection.Output, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.TotalRowsNum));


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
                // _errorCode = (Int32)cmdToExecute.Parameters["@iErrorCode"].Value;
                // objOpeningProperty.TotalRowsNum = Convert.ToInt32(cmdToExecute.Parameters["@TotalRowsNum"].Value);
                if (_errorCode != (int)LLBLError.AllOk)
                {
                    // Throw error.
                    throw new Exception("Stored Procedure 'sp_fourthTier_SelectAll' reported the ErrorCode: " + _errorCode);
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("fourthTier::SelectAll::Error occured.", ex);
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

        public DataTable SelectById()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = @"select * from ChartofAccountsChildHead where idx=@idx";
            DataTable toReturn = new DataTable("fourthTier");
            SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;
            cmdToExecute.CommandTimeout = 0;
            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@idx", SqlDbType.Int, 4, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, objOpeningProperty.idx));

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
                // _errorCode = (Int32)cmdToExecute.Parameters["@iErrorCode"].Value;
                //objOpeningProperty.TotalRowsNum = Convert.ToInt32(cmdToExecute.Parameters["@TotalRowsNum"].Value);
                if (_errorCode != (int)LLBLError.AllOk)
                {
                    // Throw error.
                    throw new Exception("Stored Procedure 'sp_fourthTier_SelectAll' reported the ErrorCode: " + _errorCode);
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("fourthTier::SelectAll::Error occured.", ex);
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

        public bool DeleteAccounts()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = @"update ChartofAccountsChildHead set visible=0 where idx=@idx";

            cmdToExecute.Connection = _mainConnection;

            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@idx", SqlDbType.Int, 100, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, objOpeningProperty.idx));

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
                    throw new Exception("Stored Procedure 'sp_update_fourthTier' reported the ErrorCode: " + _errorCode);
                }

                return true;
            }
            catch (Exception ex)
            {
                // some error occured. Bubble it to caller and encapsulate Exception object
                throw new Exception("fourthTier::Update::Error occured.", ex);
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

        public DataTable GenerateSONo(LP_GenerateTransNumber_Property objTranNo)
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_GenerateTransNo]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            DataTable toReturn = new DataTable("SO");
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
