using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    
        [RoutePrefix("api/customers")]
        public class CustomerController : ApiController
        {
            BankEntities db_entity;

            [Route("getall")]
            public HttpResponseMessage Get()
            {
                using (db_entity = new BankEntities())
                {
                    db_entity.Configuration.ProxyCreationEnabled = false;
                    return Request.CreateResponse(HttpStatusCode.OK, db_entity.SystemUser1.ToList());
                }
            }

            [Route("getcustomer")]
            public HttpResponseMessage GetbyId(int customer_Id)
            {
                using (db_entity = new BankEntities())
                {
                    db_entity.Configuration.ProxyCreationEnabled = false;
                    var customer = db_entity.SystemUser1.FirstOrDefault(c => c.ID == customer_Id);
                    if (customer == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The id mentioned does not exist");
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.Found, customer);
                }
            }

            [Route("getbranchcustomers")]
            public HttpResponseMessage GetAllCustomersinBranchbyId(int Branch_id)
            {
                using (db_entity = new BankEntities())
                {
                    db_entity.Configuration.ProxyCreationEnabled = false;
                    var customers = db_entity.SystemUser1.FirstOrDefault(c => c.BrunchCode == Branch_id);
                    //var customers = db_entity.Customers.SqlQuery("select * from Customer where Branch_id="+Branch_id);
                    if (customers == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The Branch name mentioned does not exist");
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.Found, customers);
                }
            }


            [Route("insertnew")]
            public HttpResponseMessage Post(SystemUser1 newcustomer)
            {
                using (db_entity = new BankEntities())
                {
                    try
                    {
                        db_entity.SystemUser1.Add(newcustomer);
                        db_entity.SaveChanges();
                        var msg = Request.CreateResponse(HttpStatusCode.Created, newcustomer);
                        msg.Headers.Location = new Uri(Request.RequestUri + "/" + newcustomer.ID);

                        return msg;
                    }
                    catch (Exception e)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
                    }
                }
            }

            [Route("updatecustomer")]
            public HttpResponseMessage Put(SystemUser1 ModifiedCustomer)
            {
                using (db_entity = new BankEntities())
                {
                    try
                    {
                        var cu = db_entity.SystemUser1.FirstOrDefault(c => c.ID == ModifiedCustomer.ID);
                        if (cu == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The id mentioned does not exist");
                        }
                        else
                        {
                            cu.ID = ModifiedCustomer.ID;
                            cu.Name = ModifiedCustomer.Name;
                            cu.Pasword = ModifiedCustomer.Pasword;
                            cu.Image = ModifiedCustomer.Image;
                            cu.UserType = ModifiedCustomer.UserType;
                            cu.BrunchCode = ModifiedCustomer.BrunchCode;
                            cu.Bank_branch1 = ModifiedCustomer.Bank_branch1;
                            cu.credit = ModifiedCustomer.credit;

                            db_entity.Entry(cu).State = System.Data.Entity.EntityState.Modified;
                            db_entity.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, cu);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                    }
                }
            }

            [Route("deletecustomer")]
            public HttpResponseMessage Delete(int customer_Id)
            {
                using (db_entity = new BankEntities())
                {
                    SystemUser1 cu = db_entity.SystemUser1.FirstOrDefault(c => c.ID == customer_Id);
                    if (cu == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not found");
                    else
                    {
                        db_entity.SystemUser1.Remove(cu);
                        db_entity.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Customer Deleted");
                    }
                }
            }


        }
    }
