var item;
var barChart;
var itemLine;
var PieItem;

function playAnimation(chart,effect, duration) {
    chart.startEffect = effect;
    chart.startDuration = duration;
    chart.sequencedAnimation = false;
    chart.animateAgain();
}

function makeChart(name, FunnelData, sum) {

    var chart = AmCharts.makeChart(name, {
        "type": "funnel",
        "theme": "light",
        "dataProvider": FunnelData,
        "balloon": {
            "fixedPosition": false,
            "borderColor": "#ffffff",
            "cornerRadius": 3,
            "adjustBorderColor": true,
            "borderThickness": 2,
            "fillColor": "#e4e2b6",
            "fillColor": "#FFFFFF",

            "gradientRatio": [2, 0, -0.2],

        },
        "addClassNames": true,
        "startDuration":0,
        "valueField": "value",
        "titleField": "title",
        "marginRight": 150,
        "marginLeft": 50,
        "connect": false,
        "gradientRatio": [2, 0, -0.2],
        "depth3D": 150,
        "angle": 17.6,
        "clickSlice": click,
        "outlineColor": "#FFFFFF",
        "showHandOnHover": true,

        "outlineThickness": 0,
        "colors": ["#4285D5", "#ff7400", "#ff0000", "#90B643"],
        "labelPosition": "right",
        "labelFunction": function (item, content) {
            var html = '';
            html += item.title + "," + item.dataContext.value;
            return html;
        }
,

        "balloonFunction": function (item, content) {

            var num = item.dataContext.value / sum * 100;
            var res = (num + '').split('.');
            var n;
         //   console.log(res);
            if (res.length != 1)
                n = num.toFixed(2);
            else
                n = num;

            var html = '<div>';
            html += "<p style='color:black'>" + item.title + ":" + " (" + n + "%)</p>";
            html += "</div>";
            return html;
        }
,
        "export": {
            "enabled": false
        }
    });

    chart.showHandOnHover = true;
   // playAnimation(chart, 'easeInSine', 0);


}
function click(dataItem, event) {
    PieItem = dataItem;
}


function FunnelClick() {
    return PieItem;

}

function ResetItem() {
    item = null;
}

function ResetItemPie() {
    PieItem = null;
}


function ResetItemFunnel() {
    PieItem = null;
}


    function makeBarChart(name, label, dataset, flag) {

        var ctx = document.getElementById(name);
        if (flag) {
            barChart = new Chart(ctx, {
                label: "",
                type: 'bar',
                data: {
                    labels: label,
                    datasets: [{
                        data: [65, 59, 80, 81, 56, 55, 40],


                        borderWidth: 0
                        ,
                        backgroundColor: 'rgb(72,126,159)',
                    }]
                },
                options: {
                    maintainAspectRatio: false,
                    responsive: true,
                    Tick: {
                        display: true,
                        fontColor: "#666",

                    },
                    scales: {
                        xAxes: [{
                            gridLines: {


                            },

                            categoryPercentage: 1,





                            ticks: {
                                display: true,
                                fontColor: "black",
                                fontSize: 14,
                                stepSize: 150,
                                beginAtZero: false,
                                // label: { rotation: -45 },
                                fontFamily: 'Verdana, sans-serif',
                                scaleOverride: true,
                                scaleSteps: 15,
                                scaleStartValue: 0,
                                scaleStepWidth: 3,



                            },

                            tips: {
                                trackMouse: true,
                                width: 140,
                                height: 28,

                            },




                        }],
                        yAxes: [{
                            gridLines: {
                                color: "rgba(0, 0, 0, 0)",

                            },
                            ticks: {
                                suggestedMin: 0,
                                suggestedMax: this.MaxYAxis,
                                max: this.MaxYAxis,
                                beginAtZero: true,
                                min: 0,
                                fontColor: "black",
                                fontSize: 14,
                            }
                        }]
                    },



                    legend: {
                        display: false,



                    },

                    tooltips: {
                        enabled: true,
                        backgroundColor: "#E5E5E5",
                        titleFontColor: 'black',
                        bodyFontColor: 'black',

                        callbacks: {
                            title: function (tooltipItems, data) {
                                return "";
                            },
                            label: function (tooltipItem, data) {
                                var value = tooltipItem.yLabel;
                                // var label = data.labels[tooltipItem.index];
                                //console.log(data);
                                //  var percentage = Math.round(value / 3 * 100);

                                if (tooltipItem.yLabel == 0)
                                    return "";

                                return "" + value;
                            },


                        }
                        ,

                    },








                }
            });
        }
        else {

            barChart.data.datasets[0].data = dataset;
            barChart.data.labels = label;
            barChart.update();

        }

    }
    var direction = "left";
    function makeAmBarChart(name, graphs, dataprovider, max, legendFlag, LegendDiv, minimum, stacked, IsRtl) {
        var RTL = "left";
        if (IsRtl == "right")
            RTL = IsRtl;

        if(!stacked)
        var chart = AmCharts.makeChart(name,
        {
            "type": "serial",
            "rtl": true,
            "addClassNames": true,
            "categoryField": "category",
             "autoMarginOffset": 10,
            "marginRight": 5,
            "marginTop": 5,
            "columnSpacing": 5,
            "balloonFunction": function (item, content) {
                var html = "";
                html +=customNumberFormat(item.dataContext.data);
                return html;
            },

            "legend": {
                "divId": LegendDiv,
                "showEntries": legendFlag,
                "enabled": legendFlag
            },


            "startDuration": 0,
            "showHandOnHover":true,
            "fontSize": 13,
            "theme": "default",
            "categoryAxis": {
                "gridPosition": "start",
                "tickPosition": "start",
             //   "position": "right",
                "gridColor": "#FFFFFF",
                "tickLength": 10,
                "labelRotation": 15,
                "labelFunction": function(label, item, axis) {
                    var chart = axis.chart;
                    if ( (chart.realWidth <= 100 ) && ( label.length > 2 ) )
                        return label.substr(0, 2) + '...';
                    if ( (chart.realWidth <= 100 ) && ( label.length > 2 ) )
                        return label.substr(0, 2) + '...';
                    return label;
                }

            },
            "borderAlpha": 0,
            "trendLines": [],
            "graphs": graphs
         ,
            "guides": [],
            "valueAxes": [
            {
                "position": RTL,
                //"rtl": true,
                "id": "ValueAxis-1",
                "title": "",
                "labelFunction": function (item, content) {

                    var html = "";
                    html +=customNumberFormat(item);
                    return html;
                },
                "gridColor": "#FFFFFF",
                "minorGridEnabled": true,

                "minorTickLength": 3,
                "maximum": max,
                "minimum": minimum,

            }
            ],
            "allLabels": [],
            //  "balloon": {},
            "titles": [],
            "dataProvider": dataprovider

        });
        else {
            var chart = AmCharts.makeChart(name,
      {
          "type": "serial",
          "addClassNames": true,
          "categoryField": "category",
           "autoMarginOffset": 10,
          "marginRight": 5,
          "marginTop": 5,
           "columnSpacing": 5,
           "columnWidth": 0.7,
          "balloonFunction": function (item, content) {
              var html = "";
              html += customNumberFormat(item.dataContext.data);

              return html;
          }
          ,

          "legend": {
              "divId": LegendDiv,
              "showEntries": legendFlag,
              "enabled": legendFlag
          },


          "startDuration": 0,
          "showHandOnHover": true,
          "fontSize": 13,
          "theme": "default",
          "categoryAxis": {
              "gridPosition": "start",
              "tickPosition": "start",
              "gridColor": "#FFFFFF",
              "tickLength": 10,
              "labelRotation": 15,
              "labelFunction": function (label, item, axis) {
                  var chart = axis.chart;
                  if ((chart.realWidth <= 100) && (label.length > 2))
                      return label.substr(0, 2) + '...';
                  if ((chart.realWidth <= 100) && (label.length > 2))
                      return label.substr(0, 2) + '...';
                  return label;
              }
          },
          "borderAlpha": 0,
          "trendLines": [],
          "graphs": graphs
       ,
          "guides": [],
          "valueAxes": [
          {
              "id": "ValueAxis-1",
              "title": "",
              "labelFunction": function (item, content) {

                  var html = "";
                  html += customNumberFormat(item);
                  return html;
              },
              "gridColor": "#FFFFFF",
              "minorGridEnabled": true,
              "stackType": "regular",

              "minorTickLength": 3,
              "maximum": max,
              "minimum": minimum,

          }
          ],
          "allLabels": [],
          //  "balloon": {},
          "titles": [],
          "dataProvider": dataprovider

      });
        }
        chart.addListener("clickGraphItem", handleClick);

        function handleClick(dataItem, event) {
            item = dataItem;
            //console.log(event);
        }

        if(legendFlag)
        {
            var legend = new AmCharts.AmLegend();
            chart.addLegend(legend, LegendDiv);

        }
        //  playAnimation(chart, 'easeInSine', 0.5);



    }

    function makeBanksBarChart(name, dataprovider, useLocal, IsRtl) {
        var layoutDirection = "left";
        if (IsRtl == "right")
            layoutDirection = IsRtl;

        var _isRTL = IsRtl == "right";

        var chart = AmCharts.makeChart(name, {

            "dataProvider": dataprovider,


            "type": "serial",
            "theme": "light",
            "addClassNames": true,
            "showHandOnHover": true,

            "categoryField": "Name",
            "categoryAxis": {
                "gridPosition": "start",
                "twoLineMode": true,
                "gridColor": "#FFFFFF",
                "minVerticalGap": 37,
                "title": ""
            },

            "valueAxes": [{
                "position": layoutDirection,
                "id": "ValueAxis-1",
                "stackType": "regular",
                "gridColor": "#fff",
                "title": ""
            }],

            "balloon": {
                "borderThickness": 2,
                //"cornerRadius": 5,
                "fillAlpha": 0.79,
                "shadowAlpha": 0.07,
                "shadowColor": "#575757",
            },


            //"export": {
            //    "enabled": true,
            //    "menu": [{
            //        "class": "export-main",
            //        "menu": [{
            //            "label": "Download",
            //            "menu": ["PNG", "JPG"]
            //        },
            //        {
            //            "label": "Print",
            //            "format": "PRINT"
            //        }]
            //    }]
            //}

            "graphs": [{
                "title": useLocal ? "מזומן" : "Cash",
                "valueField": "CashCol",
                "balloonText": useLocal ? "<b>סכום</b><br>[[subtitle]]<b>[[value]] [[currency]]</b>" : "<b>Amount</b><br>[[subtitle]]<b>[[value]] [[currency]]</b>",
                "fillColors": ["#BADFE8", "#7AC2D4", "#73BFD2", "#7AC2D4", "#BADFE8", ],
                "fillAlphas": 1,
                "lineAlpha": 1,
                "lineColor": "#fff",
                "type": "column",
                "gradientOrientation": "horizontal",
                "borderAlpha": 0,
                "fixedColumnWidth": 70,
                "showHandOnHover": true,
                "id": "AmGraph-95",
                "showHandOnHover": true,


            }, {
                "title": useLocal ? "דחוי" : "Postdated",
                "valueField": "PostdatedCol",
                "balloonText": useLocal ? "<b>סכום</b><br>[[subtitle]]<b>[[value]] [[currency]]</b>" : "<b>Amount</b><br>[[subtitle]]<b>[[value]] [[currency]]</b>",
                "fillColors": ["#E8EABE", "#D3C67A", "#D3C67A", "#E8EABE", ],
                "fillAlphas": 1,
                "lineAlpha": 1,
                "lineColor": "#fff",
                "type": "column",
                "gradientOrientation": "horizontal",
                "borderAlpha": 0,
                "fixedColumnWidth": 70,
                "showHandOnHover": true,
                "id": "AmGraph-96",
                "showHandOnHover": true,


            }],
        });
        console.log("Chart created", layoutDirection,_isRTL,chart)
        chart.addListener("clickGraphItem", handleClick);

        function handleClick(dataItem, event) {
            item = dataItem;
            // console.log(event);
        }


    }


    function BarClick() {
        return item;
    }

    function Lineclick(){
        return itemLine;
    }

    function ResetLineclick() {
        itemLine=null;
    }

    function PieClick() {
        return PieItem;
    }



    function makePieChart(name, data, flag, legendFlag, LegendDiv,width) {
        if (width == null)
            width = 65;
        var chart = AmCharts.makeChart(name, {
            "type": "pie",
            "theme": "light",
            "dataProvider": data,
            "addClassNames": true,
            "valueField": "data",
            "autoMargins": true,
            "titleField": "label",
            "connect": true,
            "gradientRatio": [1, 0, -0.1],
            "pullDistance": 0,
            "pullOutRadius": "0%",
            "startAngle": 55,
            "labelRadius": 15,
            "clickSlice": click,
            "maxLabelWidth": 150,
            "innerRadius": "0%",
            "legend": {
                "divId": LegendDiv,
                "showEntries": legendFlag,
                "valueWidth": 0,
                "labelWidth": width,
            },

            "showHandOnHover": true,
            "colorField": "color",

            "startDuration": 0,
            "balloon": {
                "fixedPosition": true
            },
            "labelFunction": function (item, content) {
                var html = "";
                if (flag) {
                    html += item.dataContext.label + ": " +customNumberFormat(item.dataContext.data);
                }
                return html;
            }
            ,

            "balloonFunction": function (item, content) {
                return item.dataContext.label + ":" + customNumberFormat(item.dataContext.data) + "</b>"
            }
            ,
            "export": {
                "enabled": false
            },

            "responsive": {
                "enabled": true,
                "addDefaultRules": true,

            },

             "listeners": [{
    "event": "clickSlice",
    "method": function(e) {
        PieItem = e;
        }
        }]




        });

        chart.showHandOnHover = true;

        return chart;
        // playAnimation(chart, 'easeInSine', 0.5);

    }

    function customNumberFormat(value) {
        if (value >= 1000000000)
            return (Math.round(value / 100000000 ) / 10) + "Bil";
        else if (value >= 1000000)
            return (Math.round(value / 100000) / 10) + "Mil";
        else if (value >= 1000){
            return (Math.round(value / 100)/10) + "K";
        }
        else
            return value + "";
    }

    function makeAMLineChart(name, chartData, alpha) {


        var chart = AmCharts.makeChart(name, {
            "theme": "light",
            "type": "serial",
            "marginRight": 30,
            "startDuration": 0,
            "showHandOnHover": true,
            "autoMarginOffset": 0,
            "marginTop": 10,
            "minorGridAlpha": 0,
            "minorGridEnabled": false,
            "dataProvider": chartData,
            "valueAxes": [{
                "id": "v1",
                "axisAlpha": 0.1,
            }],
            "graphs": [{
                id:"g3",
                "useNegativeColorIfDown": false,
                "bullet": "round",
                "balloonFunction": function (item, graph) {
                    return item.category + "<br><b>value:" + item.dataContext.visits + "</b>"
                },
                "bulletBorderAlpha": 1,
                "bulletBorderColor": "#FFFFFF",
                "hideBulletsCount": 50,
                "lineThickness": 2,
                "lineColor": "#3a5cba",
                "negativeLineColor": "#3a5cba",
                "valueField": "visits",
                "fillAlphas": alpha,
                "labelText": "[[visits]]",
                "color":"blue",
                "labelPosition": "top",
                "direction": "right",

            }],

            "chartCursor": {
                "valueLineEnabled": false,
                "valueLineBalloonEnabled": true
            },
            "categoryField": "date",
            "categoryAxis": {
                "parseDates": false,
                "axisAlpha": 0,
                "startOnAxis": true,
                "minHorizontalGap": 60,
                "minorGridEnabled": true,


            },
            "valueAxes": [
           {
               "minorGridEnabled": true,
               "labelFunction": function (item, content) {

                   var html = "";
                   html += customNumberFormat(item);
                   return html;
               },

           }],
            "export": {
                "enabled": false
            }
        });
        chart.addListener("clickGraphItem", handleClick);

        function handleClick(dataItem, event) {
            itemLine = dataItem;
            //console.log(event);
        }
        //playAnimation(chart, 'Bounce', 0.5);


    }


    function makeAMLineChartMultiple(name, chartData, alpha, graphs, legendFlag,LegendDiv) {


        var chart = AmCharts.makeChart(name, {
            "theme": "light",
            "type": "serial",
            "marginRight": 5,
            "startDuration": 1,
            "showHandOnHover": true,
            "autoMarginOffset": 0,
            "marginTop": 10,
            "dataProvider": chartData,
            "minorGridAlpha": 0,
            "minorGridEnabled":false,
            "valueAxes": [{
                "id": "v1",
                "axisAlpha": 0.1,
                "minorGridAlpha": 0,
                "minorGridEnabled": false,
            }],
            "graphs": graphs,
            "legend": {
                "divId": LegendDiv,
                "showEntries": legendFlag,
                "enabled": legendFlag,
                "labelText": "[[title]]",
                "valueWidth": 0,
                "useGraphSettings": true

            },
            "chartCursor": {
                "valueLineEnabled": false,
                "valueLineBalloonEnabled": true
            },
            "categoryField": "Category",
            "categoryAxis": {
                "parseDates": false,
                "axisAlpha": 0,
                "minHorizontalGap": 60,
                "minorGridAlpha": 0,
                "minorGridEnabled": false,
            },
            "valueAxes": [
           {
               "minorGridEnabled": false,
               "title": "[Hours]",
           }],

            "export": {
                "enabled": false
            }
        });


    }



