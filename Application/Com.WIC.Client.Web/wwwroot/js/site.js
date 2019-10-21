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
    .on('click', '.suggested-sentences tr>td', function (e) {
        if (e.target !== this) {
            return;
        }
        $(this).closest('tr').find(':checkbox').trigger('click');
    })
    .on('change', '.suggested-sentences :checkbox', function () {
        var oldVal = $('#SentencesToSpeak').val();
        var selectedSentence = $(this).parent().next().text().replace(/\n/g, ' ');
        if ($(this).is(':checked')) {
            if ($.trim(oldVal) !== '') {
                oldVal += '\r\n' + '\r\n';
            }
            $('#SentencesToSpeak').val($.trim(oldVal + selectedSentence));
        } else {
            $('#SentencesToSpeak').val($.trim(oldVal.replace(selectedSentence, '')));
        }
    });