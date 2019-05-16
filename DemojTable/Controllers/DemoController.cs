using DemojTable.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DemojTable.Controllers
{
    public class DemoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]//Gets the todo Lists.  
        public JsonResult GetCustomers(string companyname = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            try
            {
                var CustomerCount = GetCustomerCount();
                var Customer = GetCustomersList(companyname, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = Customer, TotalRecordCount = CustomerCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCustomerCount()
        {
            // Instance of DatabaseContext
            //Customers is DbSet and Table name in Database is Customers
            try
            {
                using (var db = new DatabaseContext())
                {
                    return db.Customers.Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Customers> GetCustomersList(string companyname, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {
                    IEnumerable<Customers> query = db.Customers;

                    //Search
                    if (!string.IsNullOrEmpty(companyname))
                    {
                        query = query.Where(p => p.CompanyName.IndexOf(companyname, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("CompanyName ASC"))
                    {
                        query = query.OrderBy(p => p.CompanyName);
                    }
                    else if (sorting.Equals("CompanyName DESC"))
                    {
                        query = query.OrderByDescending(p => p.CompanyName);
                    }
                    else if (sorting.Equals("ContactName ASC"))
                    {
                        query = query.OrderBy(p => p.ContactName);
                    }
                    else if (sorting.Equals("ContactName DESC"))
                    {
                        query = query.OrderByDescending(p => p.ContactName);
                    }
                    else if (sorting.Equals("Country ASC"))
                    {
                        query = query.OrderBy(p => p.Country);
                    }
                    else if (sorting.Equals("Country DESC"))
                    {
                        query = query.OrderByDescending(p => p.Country);
                    }
                    else if (sorting.Equals("City ASC"))
                    {
                        query = query.OrderBy(p => p.City);
                    }
                    else if (sorting.Equals("City DESC"))
                    {
                        query = query.OrderByDescending(p => p.City);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.CustomerID); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList()  //Paging
                               : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult CreateCustomer([Bind(Exclude = "CustomerID")] Customers customers)
        {
            try
            {
                //validating Model state 
                if (ModelState.IsValid)
                {
                    using (var db = new DatabaseContext())
                    {
                        // Saving Customer
                        db.Customers.Add(customers);
                        var data = db.SaveChanges();
                    }
                    // Sending Response to Ajax request
                    return Json(new { Result = "OK", Record = customers });
                }
                else
                {
                    // Generating Error Message string to send error response

                    StringBuilder msg = new StringBuilder();

                    var errormessage = (from item in ModelState
                                        where item.Value.Errors.Any()
                                        select item.Value.Errors[0].ErrorMessage).ToList();

                    for (int i = 0; i < errormessage.Count; i++)
                    {
                        msg.Append(errormessage[i].ToString());
                        msg.Append("<br />");
                    }
                    // Sending Response to Ajax request
                    return Json(new { Result = "Error occured", Message = msg.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error occured", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult EditCustomer(Customers customers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new DatabaseContext())
                    {
                        db.Entry(customers).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    StringBuilder msg = new StringBuilder();

                    var errorList = (from item in ModelState
                                     where item.Value.Errors.Any()
                                     select item.Value.Errors[0].ErrorMessage).ToList();

                    return Json(new { Result = "ERROR", Message = errorList });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult DeleteCustomer(int CustomerID)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    Customers customer = db.Customers.Find(CustomerID);
                    db.Customers.Remove(customer);
                    db.SaveChanges();
                }
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}