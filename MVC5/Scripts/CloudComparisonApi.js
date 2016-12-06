

//更新資料
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
          title: "總體維護頻率比較",
          series: [{ data: result.deviceValue }],
          categoryAxis: {
            categories: ["自己", "最大", "平均", "最小"],
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "維護頻率 : #= value #"
          }
        });

      $("#chart")
        .kendoChart({
          theme: "blueOpal",
            
          title: "廠商維護頻率比較",

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
            template: "維護頻率 : #= value #"
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
          title: "總體平均壽命比較",
          series: [{ data: result.deviceValue }],
          categoryAxis: {
            categories: ["自己", "最大", "平均", "最小"],
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "平均壽命 : #= value #"
          }
        });

      $("#chart")
        .kendoChart({
          theme: "blueOpal",

          title: "廠商平均壽命比較",

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
            template: "平均壽命 : #= value #"
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
          title: "總體更換成本比較",
          series: [{ data: result.deviceValue }],
          categoryAxis: {
            categories: ["自己", "最大", "平均", "最小"],
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "更換成本 : #= value #"
          }
        });

      $("#chart")
        .kendoChart({
          theme: "blueOpal",

          title: "廠商更換成本比較",

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
            template: "更換成本 : #= value #"
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
          title: "總體最優更換年齡比較",
          series: [{ data: result.deviceValue }],
          categoryAxis: {
            categories: ["自己", "最大", "平均", "最小"],
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "最優更換年齡 : #= value #"
          }
        });

      $("#chart")
        .kendoChart({
          theme: "blueOpal",

          title: "廠商最優更換年齡比較",

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
            template: "最優更換年齡 : #= value #"
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
          title: "總體報廢代價比較",
          series: [{ data: result.deviceValue }],
          categoryAxis: {
            categories: ["自己", "最大", "平均", "最小"],
            majorGridLines: false
          },
          tooltip: {
            visible: true,
            template: "報廢代價 : #= value #"
          }
        });

      $("#chart")
        .kendoChart({
          theme: "blueOpal",

          title: "廠商報廢代價比較",

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
            template: "報廢代價 : #= value #"
          }

        });
    }
  });
}
