using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DynamicformHome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }

        [HttpPost("SaveIem")]
        public ActionResult<BasicEntryForm> SaveIem(BasicEntryForm cty)
        {


            Type t = Type.GetType("DynamicformHome.Controllers." + cty.SourceObject);
            var obj = Activator.CreateInstance(t);             //return Ok();

            foreach (var ptr in obj.GetType()
                                .GetProperties(
                                        BindingFlags.Public
                                        | BindingFlags.Instance))
            {
                if (ptr.Name.ToLower() == cty.IDColumnName.ToLower())
                    ptr.SetValue(obj, cty.IDValue, null);
                if (cty.IsNew == false)
                {
                    if (ptr.Name.ToLower() == "isnew")
                        ptr.SetValue(obj, false, null);

                }
                if (cty.fromViewType == enumfromViewType.TreeView && cty.IDValue != null)
                {
                    if (ptr.Name.ToLower() == cty.parentColumnName.ToLower())
                        ptr.SetValue(obj, cty.IDValue, null);

                }

                foreach (var item in cty.Controls)
                {
                    if (ptr.Name.ToLower() == item.PropertyName.ToLower() && ptr.CanWrite)
                    {
                        if (item.controlType == EnumDynamicControlType.TextBox)
                            ptr.SetValue(obj, item.value, null);
                        else if (item.controlType == EnumDynamicControlType.DateTime)
                            ptr.SetValue(obj, Convert.ToDateTime(item.value), null);
                        else if (item.controlType == EnumDynamicControlType.DropDownEnumData
                            || item.controlType == EnumDynamicControlType.DropDownServerData)
                            ptr.SetValue(obj, Convert.ToInt32(item.value), null);
                        else if (item.controlType == EnumDynamicControlType.CheckBox)
                        {
                            if (item.value == "1") ptr.SetValue(obj, true, null);
                            else ptr.SetValue(obj, false, null);
                        }
                    }

                    //ptr.SetValue(obj, item.value, null);

                }
            }
            MethodInfo voidMethodInfo = t.GetMethod("Save");
            voidMethodInfo.Invoke(obj, null);

            return CreatedAtAction("", obj);

        }


        [HttpGet("GetMasterViewData/{ObjectName}/{FunctionName}/{Parameter}")]
        public ActionResult<ICollection> GetMasterViewData(string ObjectName, string FunctionName, string Parameter)
        {

            Type t = Type.GetType("DynamicformHome.Controllers." + ObjectName);
            var obj = Activator.CreateInstance(t);             //return Ok();
            string fName = "Get";
            if (FunctionName == null || FunctionName == "undefined")
            {
                fName = FunctionName;
            }

            MethodInfo voidMethodInfo = t.GetMethod(fName, new Type[0]);
            object ost = voidMethodInfo.Invoke(obj, null);
            dynamic collection = ost as IEnumerable;
            return collection;
        }

        [HttpGet("Get/{ObjectName}/{FunctionName}/{IDValue}")]
        public ActionResult<object> Get(string ObjectName, string FunctionName, int IDValue)
        {

            Type t = Type.GetType("DynamicformHome.Controllers." + ObjectName);
            var obj = Activator.CreateInstance(t);             //return Ok();
            string fName = "Get";
            if (FunctionName == null || FunctionName == "undefined")
            {
                fName = FunctionName;
            }
            object[] param = new object[1];
            param[0] = IDValue;

            Type[] paramtypes = new Type[1];
            paramtypes[0] = ((int)param[0]).GetType();

            MethodInfo voidMethodInfo = t.GetMethod(fName, paramtypes);
            object ost = voidMethodInfo.Invoke(obj, param);

            return ost;
        }

        [HttpPost("Delete")]
        public ActionResult Delete(BasicEntryForm cty)
        {

            Type t = Type.GetType("DynamicformHome.Controllers." + cty.SourceObject);
            var obj = Activator.CreateInstance(t);             //return Ok();
            string fName = "Delete";
            object[] param = new object[1];
            param[0] = cty.IDValue;

            Type[] paramtypes = new Type[1];
            paramtypes[0] = ((int)param[0]).GetType();

            MethodInfo voidMethodInfo = t.GetMethod(fName, paramtypes);
            object ost = voidMethodInfo.Invoke(obj, param);

            return Ok();
        }
    }
    public enum EnumDynamicControlType
    {
        Lebel,
        TextBox,
        DropDownEnumData,
        DropDownServerData,
        CheckBox,
        DateTime,
        NumText,
        IntNumText
    }

    public class Country
    {
        public int CountryID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime EffectDate { get; set; }
        public int ReferenceID { get; set; }
        public bool IsNew { get; set; }
        public object ParentID { get; set; }
        public bool IsSelected { get; set; }
        public void Save()
        {

        }
        public List<Country> Get()
        {
            Country oc = new Country();
            oc.CountryID = 1;
            oc.Code = "001";
            oc.Name = "bangladesh";
            oc.Address = "NA";
            oc.ReferenceID = 12;
            oc.EffectDate = new DateTime(2020, 3, 1);
            oc.IsActive = false;
            List<Country> ocs = new List<Country>();
            ocs.Add(oc);

            Country oc1 = new Country();
            oc1.CountryID = 2;
            oc1.Code = "002";
            oc1.Name = "bb";
            oc1.Address = "NA";
            oc1.ReferenceID = 12;
            oc1.EffectDate = new DateTime(2020, 3, 1);
            oc1.IsActive = false;
            oc1.ParentID = 1;
            ocs.Add(oc1);

            Country oc2 = new Country();
            oc2.CountryID = 3;
            oc2.Code = "003";
            oc2.Name = "Test";
            oc2.Address = "NA";
            oc2.ReferenceID = 12;
            oc2.EffectDate = new DateTime(2020, 3, 1);
            oc2.IsActive = false;
            oc2.ParentID = 1;
            ocs.Add(oc2);

            Country oc3 = new Country();
            oc3.CountryID = 4;
            oc3.Code = "004";
            oc3.Name = "Child of 002";
            oc3.Address = "NA";
            oc3.ReferenceID = 12;
            oc3.EffectDate = new DateTime(2020, 3, 1);
            oc3.IsActive = false;
            oc3.ParentID = 2;
            ocs.Add(oc3);

            return ocs;

        }

        public Country Get(int countryID)
        {
            Country oc = new Country();
            oc.CountryID = 1;
            oc.Code = "001";
            oc.Name = "bangladesh";
            oc.Address = "NA";
            oc.ReferenceID = 12;
            oc.EffectDate = new DateTime(2020, 3, 1);
            oc.IsActive = false;
            return oc;
        }

        public void Delete(int countryID)
        {

        }
    }

    public enum enumfromViewType
    {
        ListView,
        TreeView,
    }

    public class BasicEntryForm
    {
        public enumfromViewType fromViewType { get; set; }
        public string Caption { get; set; }
        public string SourceObject { get; set; }
        public BasicEntryFormControl[] Controls { get; set; }
        public int IDValue { get; set; }
        public bool IsNew { get; set; }
        public string IDColumnName { get; set; }
        public string parentID { get; set; }
        public string parentColumnName { get; set; }

        public string MasterViewGet { get; set; }
        public string SingleObjectGet { get; set; }

        public BasicEntryForm()
        {
            this.fromViewType = enumfromViewType.ListView;
        }
    }

    public class BasicEntryFormControl
    {
        public string value { get; set; }
        public string PropertyName { get; set; }
        public string label { get; set; }
        public string controlID { get; set; }

        public EnumDynamicControlType controlType { get; set; }

    }


}
