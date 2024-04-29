using System.Linq;
using System.Web.Mvc;
using Test9.Models; // Replace YourNamespace with your project's namespace

public class CodeController : Controller
{
    private NorthwindEntities db = new NorthwindEntities(); // Use your DbContext class name

    public ActionResult CustomersInGermany()
    {
        var customersInGermany = db.Customers.Where(c => c.Country == "Germany").ToList();
        return View(customersInGermany);
    }

    public ActionResult CustomerWithOrderId(int orderId)
    {
        var customer = db.Customers.FirstOrDefault(c => c.Orders.Any(o => o.OrderID == orderId));
        return View(customer);
    }
}
