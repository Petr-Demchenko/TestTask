<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportPage.aspx.cs" Inherits="MedTechWeb.Pages.ExportPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Экспорт данных в Excel</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            padding: 20px;
            background-color: #f5f5f5;
        }
        
        .container {
            background-color: white;
            border-radius: 5px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            padding: 30px;
            max-width: 600px;
        }
        
        .page-title {
            margin-bottom: 30px;
            color: #333;
            border-bottom: 2px solid #007bff;
            padding-bottom: 10px;
        }
        
        .export-section {
            margin: 20px 0;
        }
        
        .export-btn {
            min-width: 150px;
            font-size: 16px;
        }
        
        /* Spinner styles */
        .spinner-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            display: none;
            z-index: 9999;
            justify-content: center;
            align-items: center;
        }
        
        .spinner-overlay.show {
            display: flex;
        }
        
        .spinner-container {
            background-color: white;
            border-radius: 10px;
            padding: 30px;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
            text-align: center;
        }
        
        .spinner {
            border: 4px solid #f3f3f3;
            border-top: 4px solid #007bff;
            border-radius: 50%;
            width: 50px;
            height: 50px;
            animation: spin 1s linear infinite;
            margin: 0 auto 15px;
        }
        
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
        
        .spinner-text {
            font-size: 16px;
            color: #333;
            font-weight: 500;
        }
        
        .alert {
            margin-top: 20px;
            display: none;
        }
        
        .alert.show {
            display: block;
        }
        
        .info-box {
            background-color: #f9f9f9;
            border-left: 4px solid #007bff;
            padding: 15px;
            margin: 20px 0;
            border-radius: 3px;
        }
        
        .info-box h4 {
            margin-top: 0;
            color: #007bff;
        }
        
        .info-box ul {
            margin: 10px 0;
            padding-left: 20px;
        }
        
        .info-box li {
            margin: 5px 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1 class="page-title">Экспорт данных в Excel</h1>
            
            <div class="info-box">
                <h4>Информация</h4>
                <p>Используйте эту страницу для экспорта данных в формате Excel.</p>
                <ul>
                    <li>Нажмите кнопку "Экспорт" для загрузки данных</li>
                    <li>Во время загрузки кнопка будет неактивна</li>
                    <li>Файл автоматически загрузится в ваш браузер</li>
                </ul>
            </div>
            
            <div class="export-section">
                <asp:Button ID="btnExport" runat="server" 
                    Text="Экспорт" 
                    CssClass="btn btn-primary export-btn"
                    OnClick="BtnExport_Click" />
            </div>
            
            <div id="alertContainer">
                <asp:Literal ID="litAlert" runat="server"></asp:Literal>
            </div>
        </div>
        
        <!-- Spinner Overlay -->
        <div id="spinnerOverlay" class="spinner-overlay">
            <div class="spinner-container">
                <div class="spinner"></div>
                <div class="spinner-text">Загрузка...</div>
            </div>
        </div>
    </form>
    
    <script src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    
    <script type="text/javascript">
        function showSpinner() {
            document.getElementById('spinnerOverlay').classList.add('show');
            document.getElementById('btnExport').disabled = true;
        }
        
        function hideSpinner() {
            document.getElementById('spinnerOverlay').classList.remove('show');
            document.getElementById('btnExport').disabled = false;
        }
        
        function handleExportSuccess() {
            hideSpinner();
            showAlert('success', 'Успешно!', 'Файл успешно загружен.');
        }
        
        function handleExportError(message) {
            hideSpinner();
            showAlert('danger', 'Ошибка!', message);
        }
        
        function showAlert(type, title, message) {
            var alertClass = 'alert alert-' + type + ' show';
            var alertHtml = '<div class="' + alertClass + '" role="alert">' +
                '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
                '<span aria-hidden="true">&times;</span></button>' +
                '<strong>' + title + '</strong> ' + message + '</div>';
            
            var container = document.getElementById('alertContainer');
            container.innerHTML = alertHtml;
            
            // Auto-hide success alerts after 5 seconds
            if (type === 'success') {
                setTimeout(function() {
                    var alert = container.querySelector('.alert');
                    if (alert) {
                        alert.remove();
                    }
                }, 5000);
            }
        }
    </script>
</body>
</html>
