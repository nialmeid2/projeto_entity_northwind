
$(document).ready(function () {
    var municipio = $('#Municipio');
    var uf = $('#UF');
    var senha = $('#Usuario_Senha');
    var confirmaSenha = $('#Usuario_ConfirmarSenha');
    var erroSenha = $('#erro-senha');

    uf.change(function () {
        municipio.html('<option value="">Carregando Municípios...</option>');
        $.get(`${urlBase}/Cliente/BuscarMunicipios?uf=${uf.val()}`, function (data) {
            municipio.html('<option value="">Selecione:</option>');
            if (Array.isArray(data)) {
                for (let i = 0; i < data.length; i++) {
                    municipio.append(`<option value="${data[i].id}">${data[i].nome}</option>`)
                }
            }
        }).fail(function () {
            alert('Falha Buscando municípios, insira-o manualmente');

            municipio.removeClass('form-select').addClass('form-control');
            municipio.find('option').remove(); // como virará só um input normal, não precisa de nenhum option
            var htmlCombo = municipio.prop('outerHTML').replace(/\<select/, '<input type="text"');
            municipio.prop('outerHTML', htmlCombo);


        })
    });

    $('.form-confirma-senha').submit(function () {
        if (senha.val() != confirmaSenha.val()) {
            senha.addClass('border-danger');
            confirmaSenha.addClass('border-danger');
            erroSenha.text('A senha e a confirmação de senha devem ser iguais');

            senha.one('input', function () {
                senha.removeClass('border-danger');
                erroSenha.text('');
            });

            confirmaSenha.one('input', function () {
                confirmaSenha.removeClass('border-danger');
                erroSenha.text('');
            })
        }
    });
})