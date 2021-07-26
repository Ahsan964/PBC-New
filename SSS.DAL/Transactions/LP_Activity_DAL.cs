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
    public class LP_Activity_DAL : DBInteractionBase
    {
        private LP_Activity_Property _objACTasterProperty;

        public LP_Activity_DAL(LP_Activity_Property objACTasterProperty)
        {
            _objACTasterProperty = objACTasterProperty;
        }
        public override bool Insert()
        {
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[sp_localPurchase_Insert]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;

            // Use base class' connection object
            cmdToExecute.Connection = _mainConnection;
            try
            {
                cmdToExecute.Parameters.Add(new SqlParameter("@activityDate", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objACTasterProperty.activityDate));
                cmdToExecute.Parameters.Add(new SqlParameter("@creationDate", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, _objACTasterProperty.creationDate));

                cmdToExecute.Parameters.Add(new SqlParameter("@createdBy", SqlDbType.Int, 80, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objACTasterProperty.createdBy));
                cmdToExecute.Parameters.Add(new SqlParameter("@orderIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objACTasterProperty.orderIdx));
                cmdToExecute.Parameters.Add(new SqlParameter("@productIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 10, 0, "", DataRowVersion.Proposed, _objACTasterProperty.productIdx));
                cmdToExecute.Parameters.Add(new SqlParameter("@vendorCatIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objACTasterProperty.vendorCatIdx));

                cmdToExecute.Parameters.Add(new SqlParameter("@vendorIdx", SqlDbType.Int, 50, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Proposed, _objACTasterProperty.vendorIdx));
                cmdToExecute.Parameters.Add(new SqlParameter("@size", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objACTasterProperty.size));
                cmdToExecute.Parameters.Add(new SqlParameter("@qty", SqlDbType.Decimal, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objACTasterProperty.qty));
                
                cmdToExecute.Parameters.Add(new SqlParameter("@activityPrice", SqlDbType.Decimal, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objACTasterProperty.activityPrice));
                cmdToExecute.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar, 50, ParameterDirection.Input, true, 18, 1, "", DataRowVersion.Proposed, _objACTasterProperty.description));
                cmdToExecute.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int, 32, ParameterDirection.Output, true, 10, 0, "", DataRowVersion.Proposed, _objACTasterProperty.idx));

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
