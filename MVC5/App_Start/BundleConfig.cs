﻿using System.Web;
using System.Web.Optimization;

namespace MVC5
{
  public class BundleConfig
  {
    // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                  "~/Scripts/jquery-{version}.js"));

      bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                  "~/Scripts/kendo.all.min.js",
                  "~/Scripts/kendo.aspnetmvc.min.js"));

      bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                  "~/Scripts/jquery.validate*"));

      bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                  "~/Scripts/modernizr-*"));

      bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

      bundles.Add(new ScriptBundle("~/bundles/jquery-confirm2").Include(
                      "~/bower_components/jquery-confirm2/js/jquery-confirm.js"));

      bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/kendo.bootstrap.min.css",
                "~/Content/kendo.common-bootstrap.min",
                "~/Content/kendo/2016.3.1028/kendo.common.min.css",
                "~/Content/kendo/2016.3.1028/kendo.mobile.all.min.css",
                "~/Content/kendo/2016.3.1028/kendo.dataviz.min.css",
                "~/Content/kendo/2016.3.1028/kendo.default.min.css",
                "~/Content/kendo/2016.3.1028/kendo.dataviz.default.min.css",
                "~/bower_components/jquery-confirm2/css/jquery-confirm.css",
                "~/Content/site.css"));
    }
  }
}
