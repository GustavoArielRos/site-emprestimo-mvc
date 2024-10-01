

$(document).ready(function () {

    $('#Emprestimos').DataTable({
        language:
            {
                
                    "decimal": "",
                    "emptyTable": "No data available in table",
                    "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                    "infoEmpty": "Showing 0 to 0 of 0 entries",
                    "infoFiltered": "(filtered from _MAX_ total entries)",
                    "infoPostFix": "",
                    "thousands": ",",
                    "lengthMenu": "Show _MENU_ entries",
                    "loadingRecords": "Loading...",
                    "processing": "",
                    "search": "Search:",
                    "zeroRecords": "No matching records found",
                    "paginate": {
                        "first": "First",
                        "last": "Last",
                        "next": "Next",
                        "previous": "Previous"
                    },
                    "aria": {
                        "orderable": "Order by this column",
                        "orderableReverse": "Reverse order this column"
                    }
            }
           
    });

    //função que permite executar um código em um período de tempo determinado, nesse caso são uns 5 segundos
    setTimeout(function () {
        //usando o jQuery para selecionar todos os elementos da aplicação que são alert
        //slow é para o aparecimento ser lento
        $(".alert").fadeOut("slow", function () {
            $(this).alert("close");
        })
    }, 5000)


});
