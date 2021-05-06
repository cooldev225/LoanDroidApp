"use strict";

// Shared Colors Definition
const primary = '#6993FF';
const success = '#1BC5BD';
const info = '#8950FC';
const warning = '#FFA800';
const danger = '#F64E60';
var chart_client, chart_investor, chart_finance;
var KTApexChartsDemo = function () {
	// Private functions
	var _init_client = function () {
		var options_pack = {
			series: [{
				name: "Clients",
				data: [10, 41, 35, 51, 49, 62, 69, 91, 148, 1, 1, 1]
			}],
			chart: {
				height: 350,
				type: 'line',
				zoom: {
					enabled: false
				}
			},
			dataLabels: {
				enabled: false
			},
			stroke: {
				curve: 'straight'
			},
			grid: {
				row: {
					colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
					opacity: 0.5
				},
			},
			xaxis: {
				categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
			},
			colors: [primary]
		};
		chart_client = new ApexCharts(document.querySelector("#chart_client"), options_pack);
		chart_client.render();
	}

	var _init_investor = function () {
		const apexChart = "#chart_investor";
		var options = {
			series: [{
				name: "Investors",
				data: [10, 41, 35, 51, 49, 62, 69, 91, 148, 12, 78, 24]
			}],
			chart: {
				height: 350,
				type: 'line',
				zoom: {
					enabled: false
				}
			},
			dataLabels: {
				enabled: false
			},
			stroke: {
				curve: 'straight'
			},
			grid: {
				row: {
					colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
					opacity: 0.5
				},
			},
			xaxis: {
				categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
			},
			colors: [primary]
		};

		chart_finance = new ApexCharts(document.querySelector(apexChart), options);
		chart_finance.render();
	}

	var _init_fincance = function () {
		const apexChart = "#chart_finance";
		var options = {
			series: [{
				name: 'Loans',
				data: [31, 40, 28, 51, 42, 109, 100]
			}, {
				name: 'Interests',
				data: [11, 41, 145, 32, 34, 59, 41]
			}, {
				name: 'Investments',
				data: [17, 57, 22, 32, 134, 52, 57]
			}, {
				name: 'Saving',
				data: [1, 32, 45, 32, 38, 52, 141]
			}],
			chart: {
				height: 350,
				type: 'area',
				zoom: {
					enabled: false
				}
			},
			dataLabels: {
				enabled: false
			},
			stroke: {
				curve: 'straight'
			},
			xaxis: {
				categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
			},
			tooltip: {
				x: {
					format: 'dd/MM/yy HH:mm'
				},
			},
			colors: [primary, success, info, warning, danger, primary, success, info, warning, danger, primary, success, info, warning, danger, primary, success, info, warning, danger, primary, success, info, warning, danger]
		};

		chart_investor = new ApexCharts(document.querySelector(apexChart), options);
		chart_investor.render();
	}


	return {
		// public functions
		init: function () {
			_init_client();
			_init_investor();
			_init_fincance();
		}
	};
}();

jQuery(document).ready(function () {
	$('#chart_client_date').datepicker({
		rtl: KTUtil.isRTL(),
		todayHighlight: true,
		orientation: 'bottom left',
		format: "yyyy-mm",
		viewMode: "months",
		minViewMode: "months"
	});
	$('#chart_investor_date').datepicker({
		rtl: KTUtil.isRTL(),
		todayHighlight: true,
		orientation: 'bottom left',
		format: "yyyy-mm",
		viewMode: "months",
		minViewMode: "months"
	});
	$('#chart_finance_date').datepicker({
		rtl: KTUtil.isRTL(),
		todayHighlight: true,
		orientation: 'bottom left',
		format: "yyyy-mm",
		viewMode: "months",
		minViewMode: "months"
	});
	KTApexChartsDemo.init();
	//changeClient();
	//changeInvestor();
	//changeFinance();
});
function changeClient() {
	var form_data = new FormData();
	form_data.append('date', $('#chart_client_date').val());
	$.ajax({
		url: '/admin/getClientsByMonth',
		headers: {
			'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
		},
		data: form_data,
		cache: false,
		contentType: false,
		processData: false,
		type: 'POST',
		dataType: "json",
		success: function (response) {
			chart_client.updateSeries([{
				name: "Client",
				data: response,
			}]);
		},
		error: function (response) {

		}
	});
}
$('#chart_client_date').on('change', function () { changeClient(); });
function changeInvestor() {
	var form_data = new FormData();
	form_data.append('date', $('#chart_investor_date').val());
	$.ajax({
		url: '/admin/getInvestorByMonth',
		headers: {
			'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
		},
		data: form_data,
		cache: false,
		contentType: false,
		processData: false,
		type: 'POST',
		dataType: "json",
		success: function (response) {
			var data = new Array(response.length - 1);
			for (var i = 1; i < response.length; i++)data[i - 1] = {
				name: response[0][i - 1],
				data: response[i],
			}
			chart_investor.updateSeries(data);
		},
		error: function (response) {

		}
	});
}
$('#chart_investor_date').on('change', function () { changeInvestor(); });
function changeFinance() {
	var form_data = new FormData();
	form_data.append('date', $('#chart_finance_date').val());
	form_data.append('user', $('#chart_user_name').val());
	$.ajax({
		url: '/admin/getFinanceByMonth',
		headers: {
			'X-CSRF-Token': $('meta[name="csrf-token"]').attr('content')
		},
		data: form_data,
		cache: false,
		contentType: false,
		processData: false,
		type: 'POST',
		dataType: "json",
		success: function (response) {
			var data = new Array(response.length);
			for (var i = 1; i < response.length; i++)data[i] = i + 1;
			var options = {
				series: [{
					name: "Finance",
					data: response
				}],
				chart: {
					height: 350,
					type: 'line',
					zoom: {
						enabled: false
					}
				},
				dataLabels: {
					enabled: false
				},
				stroke: {
					curve: 'straight'
				},
				grid: {
					row: {
						colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
						opacity: 0.5
					},
				},
				xaxis: {
					categories: data
				},
				colors: [primary]
			};
			chart_finance.updateOptions(options);
		},
		error: function (response) {

		}
	});
}
$('#chart_finance_date').on('change', function () { changeFinance(); });
$('#chart_user_name').on('change', function () { changeFinance(); });