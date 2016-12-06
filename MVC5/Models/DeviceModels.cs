using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace MVC5.Models
{
  public class Device
  {
    [DisplayName("ID")]
    [Key]
    [Required]
    public int ID { get; set; }

    [DisplayName("设备编号")]
    public int DeviceId { get; set; }

    [DisplayName("设备名称")]
    [StringLength(100)]
    [Required]
    public string Name { get; set; }

    [DisplayName("设备类别")]
    [StringLength(100)]
    public string Type { get; set; }

    [DisplayName("设备厂商")]
    [StringLength(100)]
    public string Factory { get; set; }

    [DisplayName("设备模型")]
    [StringLength(100)]
    public string Model { get; set; }

    [DisplayName("设备数量")]
    public int Amount { get; set; }

    [DisplayName("设备说明")]
    [StringLength(100)]
    public string Description { get; set; }

    [DisplayName("维护频率 (x次 / 每年)")]
    public double MaintainFrequency { get; set; }

    [DisplayName("更换/翻新综合成本(每台)")]
    public int ReplaceFee { get; set; }

    [DisplayName("设备最大可能年限")]
    public int MaxUsedYear { get; set; }

    [DisplayName("上传年份列表")]
    public bool DeviceList { get; set; }

    [DisplayName("平均更换年龄")]
    public double AvgChangeYear { get; set; }

    [DisplayName("最优更换年龄")]
    public double OptChangeYear { get; set; }

    [DisplayName("报废代价")]
    public double ScrapCost { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }
  }
}

