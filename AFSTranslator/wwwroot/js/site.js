$(document).ready(function () {
    $('#translationLogsTable').DataTable({
        ajax: {
            url: '/Translate/GetUserTranslationLogs',
            dataSrc: ''
        },
        columns: [
            { data: 'originalText' },
            { data: 'translatedText' },
            { data: 'mode' },
            { 
                data: 'createdAt',
                render: function (data) {
                    return new Date(data).toLocaleString(); 
                }
            }
        ]
    });
});

