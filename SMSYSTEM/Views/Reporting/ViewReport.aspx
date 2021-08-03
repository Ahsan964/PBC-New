<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReports.aspx.cs" Inherits="SMSYSTEM.Views.Reporting.Report" %>
<%@ Import Namespace="SMSYSTEM.Views.Reporting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <!-- Required meta tags -->
    
</head>
<body>
<meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <title>@ViewBag.Title</title>
    <!-- plugins:css -->
    <link rel="stylesheet" src="/node_module/mdi/css/materialdesignicons.min.css" type="text/css" />
    <link rel="stylesheet" src="~/node_module/simple-line-icons/css/simple-line-icons.css"/>
    <link rel="stylesheet" src="~/node_module/flag-icon-css/css/flag-icon.min.css" />
    <link rel="stylesheet" src="~/node_module/perfect-scrollbar/dist/css/perfect-scrollbar.min.css" />
    <link src="~/node_module/sweetalert2/sweetalert2.min.css" rel="stylesheet" />
    <link rel="stylesheet" src="~/node_module/datatables.net-bs4/css/dataTables.bootstrap4.css" />

    <style>
        table.dataTable thead tr {
            background-color: #03A9F3;
            color: #F7F7F7;
            font-family: "roboto-regular", sans-serif;
            width: 100%;
        }


        .loader {
            position: fixed;
            z-index: 99;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: white;
            display: flex;
            justify-content: center;
            align-items: center;
        }

            .loader > img {
                width: 80px;
            }

            .loader.hidden {
                background: white;
                animation: fadeOut 3s !important;
                animation-fill-mode: forwards !important;
                z-index: -9999;
            }

        }
    </style>
    <!-- endinject -->
    <!-- plugin css for this page -->
    <link rel="stylesheet" src"/node_module/font-awesome/css/font-awesome.min.css"/>
    <link rel="stylesheet" src"/node_module/jquery-bar-rating/dist/themes/fontawesome-stars.css">
    <!-- End plugin css for this page -->
    <!-- plugin css for this page -->
    <link rel="stylesheet" type="text/css" src="/node_module/sweetalert2/dist/sweetalert2.min.css">
    <!-- End plugin css for this page -->
    <!-- inject:css -->
    <link  type="text/css" src="/Dist/css/style.css"/>
    <%--<link rel="stylesheet" href="@Url.Content("~/Dist/css/style.css")" type="text/css">--%>
    <!-- endinject -->
    <link rel="shortcut icon" src="../Shared/images/favicon.png"/>
   <%-- <title></title>--%>

<% WebFormMVCUtil.RenderPartial("_navbar", null ); %>
<% WebFormMVCUtil.RenderPartial("_sidebar", null ); %>
    <form id="form1" runat="server">
    <div>
    This is form
    </div>
    </form>

  <!-- plugins:js -->
<script type="text/javascript" src="/node_module/jquery/dist/jquery.min.js"></script>
    <script src="/node_module/popper.js/dist/umd/popper.min.js"></script>
    <script src="/node_module/bootstrap/dist/js/bootstrap.min.js"></script>
    <script src="/node_module/perfect-scrollbar/dist/js/perfect-scrollbar.jquery.min.js"></script>
    <script src="/node_module/datatables.net/js/jquery.dataTables.js"></script>
    <script src="/node_module/datatables.net-bs4/js/dataTables.bootstrap4.js"></script>
    <!-- endinject -->
    <script src="/Dist/js/jquery.validate.min.js"></script>
    <script src="/Dist/js/jquery.validate.unobtrusive.min.js"></script>
    <!-- Plugin js for this page-->
    <!-- Plugin js for this page-->

    <script src="/node_module/sweetalert2/sweetalert2.min.js"></script>
  
    <script src="/node_module/jquery.avgrund/jquery.avgrund.min.js"></script>
    <!-- End plugin js for this page-->
    <script src="/node_module/jquery-bar-rating/dist/jquery.barrating.min.js"></script>
    <script src="/node_module/chart.js/dist/Chart.min.js"></script>
    <script src="/node_module/raphael/raphael.min.js"></script>
    <script src="/node_module/morris.js/morris.min.js"></script>
    <script src="/node_module/jquery-sparkline/jquery.sparkline.min.js"></script>
    <!-- End plugin js for this page-->
    <!--
    DAtatable -->
    <script src="/node_module/datatables.net/js/jquery.dataTables.js"></script>
    <script src="/node_module/datatables.net-bs4/js/dataTables.bootstrap4.js"></script>
    <!--End DATATABLE-->
    <!-- inject:js -->
    <script src="/Dist/js/off-canvas.js"></script>
    <script src="/Dist/js/hoverable-collapse.js"></script>
    <script src="/Dist/js/misc.js"></script>
    <script src="/Dist/js/settings.js"></script>
    <script src="/Dist/js/todolist.js"></script>
    <!-- endinject -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <link href="/node_module/toastr/toastr.min.css" rel="stylesheet" />
    <script src="/node_module/toastr/toastr.min.js"></script>
    <!-- Custom js for this page-->
    <script src="/Dist/js/dashboard.js"></script>
    <!-- End custom js for this page-->
    <script src="/Dist/js/data-table.js"></script>
    <!-- Custom js for this page-->
    <script src="/Dist/js/alerts.js"></script>
    <script src="/Dist/js/avgrund.js"></script>

    <script>
        window.addEventListener("load", function () {
            var loader = document.getElementById("loader");
           loader.className += " hidden"; // class "loader hidden"
          //  $(".loader").hide();
        });

    </script>

</body>
</html>
