$(document).ready(function () {
    FillDoYouKnowQuestions();
});


function FillDoYouKnowQuestions() {
    var expertGUID = document.getElementById("expertGUID").value;; // Get expertGUID from hidden input

    if (!expertGUID) {
        console.log("ExpertGUID is missing!");
        return;
    }

    $.ajax({
        url: "/ApplicationForm/GetExpertQuestionAnswers",
        type: "GET",
        data: { expertGUID: expertGUID },
        success: function (response) {
            if (response.Valid) {
                SetFormValues(response.result);
            } else {
                console.log("No previous answers found.");
            }
        },
        error: function (err) {
            console.log("Error fetching answers:", err);
        }
    });
}

function SetFormValues(answers) {
    answers.forEach(function (answer) {
        var questionID = answer.questionID;
        var answerValue = answer.answer; // true or false

        // Select the corresponding radio button
        var yesRadio = $("#question_" + questionID + "_yes");
        var noRadio = $("#question_" + questionID + "_no");

        if (answerValue) {
            yesRadio.prop("checked", true);
        } else {
            noRadio.prop("checked", true);
        }
    });

    console.log("Form prefilled successfully!");
}

function validateAnswers() {
    const questions = document.querySelectorAll('[id^="question_"]');
    console.log("Total questions found:", questions.length);

    // Create a set to track answered questions
    const answeredQuestions = new Set();

    // Loop through all answer inputs
    questions.forEach(input => {
        if (input.checked) {
            const questionId = input.name.split('_')[1]; // Extract question ID
            answeredQuestions.add(questionId);
        }
    });

    // Check if all questions have been answered
    const totalQuestions = new Set([...questions].map(input => input.name.split('_')[1])); // Get unique question IDs
    if (answeredQuestions.size !== totalQuestions.size) {
        swal({
            title: "⚠️ تنبيه!",
            text: "يجب الإجابة على جميع الأسئلة قبل الإرسال.",
            icon: "error",
            button: "موافق"
        });
        return false; // Validation failed
    }

    return true; // Validation passed
}

function submitAnswers() {
    if (!validateAnswers()) {
        return; // Stop submission if validation fails
    }

    const answers = [];
    var expertID = document.getElementById("expertGUID").value;
    console.log("Expert GUID:", expertID);

    document.querySelectorAll('[id^="question_"]').forEach(input => {
        if (input.checked) {
            const questionId = input.name.split('_')[1];
            const answer = input.value === 'true';

            const answerObj = {
                expertGUID: expertID,
                questionID: parseInt(questionId),
                answer: answer
            };

            answers.push(answerObj);
        }
    });

    console.log("Final Answers Array:", answers);

    const url = '/ApplicationForm/SaveExpertQuestionAnswers';
    const parameters = JSON.stringify(answers);

    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: parameters,
        success: function (result) {
            console.log('Server Response:', result);

            if (result.Valid) {
                swal({
                    title: "✔️ تم بنجاح!",
                    text: "تم إرسال إجاباتك بنجاح.",
                    icon: "success",
                    button: "موافق"
                })
            } else {
                swal({
                    title: "❌ فشل الإرسال",
                    text: result.ServiceError || "حدث خطأ ما، يرجى المحاولة مرة أخرى.",
                    icon: "error",
                    button: "حاول مرة أخرى"
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            swal({
                title: "⚠️ فشل الطلب",
                text: "تعذر إرسال الإجابات. يرجى التحقق من اتصالك وإعادة المحاولة.",
                icon: "error",
                button: "موافق"
            });
        }
    });
}
