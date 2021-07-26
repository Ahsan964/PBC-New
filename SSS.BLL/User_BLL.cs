using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSS.DAL;
using SSS.Property;
using SSS.DAL;
using System.Data;
using System.Xml.Linq;
using SSS.Property.Setups;

namespace SSS.BLL
{
    public class User_BLL 
    {
        private User_DAL objUserDAL;
        private User_Property objUserProperty;

        private int? id;
        public User_BLL()
        {
         

        }
        public User_BLL(User_Property obCompany_Property)
        {
            objUserProperty = obCompany_Property;

        }
        public User_BLL(int? id)
        {
            this.id = id;
        }
        public DataTable ViewAll()
        {
            objUserDAL = new User_DAL(objUserProperty);
            return objUserDAL.SelectAll();
        }
        public DataTable SelectBranch()
        {
            objUserDAL = new User_DAL(objUserProperty);
            return objUserDAL.SelectByBranchId();
        }

        public DataTable SelectDepartment()
        {
            objUserDAL = new User_DAL(objUserProperty);
            return objUserDAL.SelectByDepartmentId();
        }

        public DataTable GetUserPagsAccess()
        {
            objUserDAL = new User_DAL(objUserProperty);
            return objUserDAL.GetUserPagsAccess();
        }
        public bool UpdatePageUser(LP_PageUser_Property objpageuser)
        {
            objUserDAL = new User_DAL();
            return objUserDAL.UpdatePageUser(objpageuser);
        }
        public DataTable SelectByIDPassword()
        {
            objUserDAL = new User_DAL(objUserProperty);
            return objUserDAL.SelectByIDPassword();
        }
        public DataTable GetById()
        {
            objUserDAL = new User_DAL(objUserProperty);
            return objUserDAL.SelectById();
        }
        public bool Insert()
        {
            objUserDAL = new User_DAL(objUserProperty);
            return objUserDAL.Insert();
        }
        public bool Update()
        {
            objUserDAL = new User_DAL(objUserProperty);
            return objUserDAL.Update();
        }

        public bool Delete(int? id)
        {
            objUserDAL = new User_DAL(objUserProperty);
            return objUserDAL.Delete(id);
        }
        public DataTable GetAllPages()
        {
            objUserDAL = new User_DAL();
            return objUserDAL.GetAllPages();
        }

        public DataTable ViewAll(string xmlpath)
        {

            // load your xml file (this one is named people and it is in my App_Data folder)
            XElement x = XElement.Load(xmlpath);//get your file
                                                // declare a new DataTable and pass your XElement to it


            objUserDAL = new User_DAL(objUserProperty);
            return objUserDAL.XElementToDataTable(x);
        }

      
    }
}
