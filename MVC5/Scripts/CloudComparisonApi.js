

//更新资料
function GetMaintainFrequency(controllerName, id) {
  $.ajax({
    url: '/' + controllerName + '/GetMaintainFrequency/' + id,
    cache: false,
    type: 'GET',
    dataType: "json",
    contentType: 'application/json; charset=utf-8',
    success: function (result) {
      var seriestemp = [];
      for (var i = 0; i < result.deviceValue.length; i++) {
        seriestemp.push({
          type: "bar",
          data: result.deviceValue[i]
        });
      }

      $("#chart1")
        .kendoChart({
          title: "总体维护频率比较",
          series: [{ data: result.deviceValue }],
          categoryAxis: {
            categories: ["自己", "最大", "平均", "最小"],
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "维护频率 : #= value #"
          }
        });

      $("#chart")
        .kendoChart({
          theme: "blueOpal",

          title: "厂商维护频率比较",

          series: [
          {
            type: "bar",
            data: result.factoryValue
          }],
          categoryAxis: {
            categories: result.factory,
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "维护频率 : #= value #"
          }

        });
    }
  });
}

function GetAverageAge(controllerName, id) {
  $.ajax({
    url: '/' + controllerName + '/GetAverageAge/' + id,
    cache: false,
    type: 'GET',
    dataType: "json",
    contentType: 'application/json; charset=utf-8',
    success: function (result) {
      var seriestemp = [];
      for (var i = 0; i < result.deviceValue.length; i++) {
        seriestemp.push({
          type: "bar",
          data: result.deviceValue[i]
        });
      }

      $("#chart1")
        .kendoChart({
          title: "总体平均寿命比较",
          series: [{ data: result.deviceValue }],
          categoryAxis: {
            categories: ["自己", "最大", "平均", "最小"],
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "平均寿命 : #= value #"
          }
        });

      $("#chart")
        .kendoChart({
          theme: "blueOpal",

          title: "厂商平均寿命比较",

          series: [
          {
            type: "bar",
            data: result.factoryValue
          }],
          categoryAxis: {
            categories: result.factory,
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "平均寿命 : #= value #"
          }

        });
    }
  });
}

function GetReplacefee(controllerName, id) {
  $.ajax({
    url: '/' + controllerName + '/GetReplacefee/' + id,
    cache: false,
    type: 'GET',
    dataType: "json",
    contentType: 'application/json; charset=utf-8',
    success: function (result) {
      var seriestemp = [];
      for (var i = 0; i < result.deviceValue.length; i++) {
        seriestemp.push({
          type: "bar",
          data: result.deviceValue[i]
        });
      }

      $("#chart1")
        .kendoChart({
          title: "总体更换成本比较",
          series: [{ data: result.deviceValue }],
          categoryAxis: {
            categories: ["自己", "最大", "平均", "最小"],
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "更换成本 : #= value #"
          }
        });

      $("#chart")
        .kendoChart({
          theme: "blueOpal",

          title: "厂商更换成本比较",

          series: [
          {
            type: "bar",
            data: result.factoryValue
          }],
          categoryAxis: {
            categories: result.factory,
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "更换成本 : #= value #"
          }

        });
    }
  });
}

function GetOptChangeYear(controllerName, id) {
  $.ajax({
    url: '/' + controllerName + '/GetOptChangeYear/' + id,
    cache: false,
    type: 'GET',
    dataType: "json",
    contentType: 'application/json; charset=utf-8',
    success: function (result) {
      var seriestemp = [];
      for (var i = 0; i < result.deviceValue.length; i++) {
        seriestemp.push({
          type: "bar",
          data: result.deviceValue[i]
        });
      }

      $("#chart1")
        .kendoChart({
          title: "总体最优更换年龄比较",
          series: [{ data: result.deviceValue }],
          categoryAxis: {
            categories: ["自己", "最大", "平均", "最小"],
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "最优更换年龄 : #= value #"
          }
        });

      $("#chart")
        .kendoChart({
          theme: "blueOpal",

          title: "厂商最优更换年龄比较",

          series: [
          {
            type: "bar",
            data: result.factoryValue
          }],
          categoryAxis: {
            categories: result.factory,
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "最优更换年龄 : #= value #"
          }

        });
    }
  });
}

function GetScrapCost(controllerName, id) {
  $.ajax({
    url: '/' + controllerName + '/GetScrapCost/' + id,
    cache: false,
    type: 'GET',
    dataType: "json",
    contentType: 'application/json; charset=utf-8',
    success: function (result) {
      var seriestemp = [];
      for (var i = 0; i < result.deviceValue.length; i++) {
        seriestemp.push({
          type: "bar",
          data: result.deviceValue[i]
        });
      }

      $("#chart1")
        .kendoChart({
          title: "总体报废代价比较",
          series: [{ data: result.deviceValue }],
          categoryAxis: {
            categories: ["自己", "最大", "平均", "最小"],
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "报废代价 : #= value #"
          }
        });

      $("#chart")
        .kendoChart({
          theme: "blueOpal",

          title: "厂商报废代价比较",

          series: [
          {
            type: "bar",
            data: result.factoryValue
          }],
          categoryAxis: {
            categories: result.factory,
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "报废代价 : #= value #"
          }

        });
    }
  });
}

