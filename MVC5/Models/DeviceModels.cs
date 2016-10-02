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

    [DisplayName("設備編號")]
    [Required]
    public int DeviceId { get; set; }

    [DisplayName("設備名稱")]
    [StringLength(100)]
    [Required]
    public string Name { get; set; }

    [DisplayName("設備類別")]
    [StringLength(100)]
    public string Type { get; set; }

    [DisplayName("設備廠商")]
    [StringLength(100)]
    public string Factory { get; set; }

    [DisplayName("設備模型")]
    [StringLength(100)]
    public string Model { get; set; }

    [DisplayName("設備數量")]
    public int Amount { get; set; }

    [DisplayName("設備說明")]
    [StringLength(10)]
    public string Description { get; set; }

    [DisplayName("維護頻率 (x次 / 每年)")]
    public double MaintainFrequency { get; set; }

    [DisplayName("更換/翻新綜合成本(每台)")]
    public int ReplaceFee { get; set; }

    [DisplayName("設備最大可能年限")]
    public int MaxUsedYear { get; set; }
    
    public virtual ApplicationUser ApplicationUser { get; set; }
  }
}