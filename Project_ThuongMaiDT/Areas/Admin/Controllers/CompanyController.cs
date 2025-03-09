using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMDT.DataAccess.Data;
using TMDT.DataAccess.Repository.IRepository;
using TMDT.Models;
using TMDT.Models.ViewModels;
using TMDT.Utility;


namespace Project_ThuongMaiDT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            List<Company> objCompanylist = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanylist);
        }
        public IActionResult Upsert(int? id)
        {
            
            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {
                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company create successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(CompanyObj);

            }
        }
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    //Company CompanyFromDb1 = _dB.categories.FirstOrDefault(u => u.Id==id);
        //    //Company CompanyFromDb2 = _dB.categories.Where(u => u.Id==id).FirstOrDefault();
        //    if (CompanyFromDb == null) { return NotFound(); }

        //    return View(CompanyFromDb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Company obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Company update successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (CompanyFromDb == null) { return NotFound(); }

        //    return View(CompanyFromDb);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Company obj = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (obj == null) { return NotFound(); }
        //    _unitOfWork.Company.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Company deleted successfully";
        //    return RedirectToAction("Index");
        //}
        public IActionResult Details(int id)
        {
            var company = _unitOfWork.Company.Get(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return PartialView("_CompanyDetails", company);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanylist = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanylist });     
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u =>u.Id == id);   
            if(CompanyToBeDeleted == null) 
            { 
                return Json(new {success = false, message = "Error while deleting"}); 
            }
            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = false, message = "Error successful" });
        }
        #endregion

    }
 }
