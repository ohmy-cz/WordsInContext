// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document)
    .on('change', '#showsentenceentry', function () {
        if (!$('#wordentry').hasClass('d-none')) {
            $('#wordentry').addClass('d-none');
            $('#sentenceentry').removeClass('d-none');
            $('#wordentry').find('input').removeAttr('required');
            $('#sentenceentry').find('textarea').prop('required', true);
        } else {
            $('#wordentry').removeClass('d-none');
            $('#sentenceentry').addClass('d-none');
            $('#wordentry').find('input').prop('required', true);
            $('#sentenceentry').find('textarea').removeAttr('required');
        }
    })
    .on('change', '.suggested-sentences :checkbox', function () {
        var oldVal = $('#SentencesToSpeak').val();
        var selectedSentence = $(this).parent().next().text();
        if ($(this).is(':checked')) {
            if ($.trim(oldVal) !== '') {
                oldVal += '\r\n' + '\r\n';
            }
            $('#SentencesToSpeak').val($.trim(oldVal + selectedSentence));
        } else {
            $('#SentencesToSpeak').val($.trim(oldVal.replace(selectedSentence, '')));
        }
    });