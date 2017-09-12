'use strict';

(function () {
    var form = document.querySelector('#cardForm');
    var clientToken = '';

    function getClientToken() {
        $.get('http://localhost:5000/api/braintree/GetClientToken', (response) => {
            clientToken = response[1];

            braintree.client.create({
                authorization: clientToken
            }, function (err, clientInstance) {
                if (err) {
                    console.error(err);
                    return;
                }
                createHostedFields(clientInstance);
            });
        });
    };
    getClientToken();

    function createHostedFields(clientInstance) {
        braintree.hostedFields.create({
            client: clientInstance,
            styles: {
                'input': {
                    'font-size': '16px',
                    'font-family': 'courier, monospace',
                    'font-weight': 'lighter',
                    'color': '#ccc'
                },
                ':focus': {
                    'color': 'black'
                },
                '.valid': {
                    'color': '#8bdda8'
                }
            },
            fields: {
                number: {
                    selector: '#card-number',
                    placeholder: '4111 1111 1111 1111'
                },
                cvv: {
                    selector: '#cvv',
                    placeholder: '123'
                },
                expirationDate: {
                    selector: '#expiration-date',
                    placeholder: 'MM/YYYY'
                },
                postalCode: {
                    selector: '#postal-code',
                    placeholder: '11111'
                }
            }
        }, function (err, hostedFieldsInstance) {
            var teardown = function (event) {
                event.preventDefault();
                hostedFieldsInstance.tokenize(function (tokenizeErr, payload) {
                    if (tokenizeErr) {
                        console.error(tokenizeErr);
                        return;
                    }

                    var data = JSON.stringify({
                        amount: 39.99,
                        client_token: clientToken,
                        payment_method_nonce: payload.nonce,
                    });

                    console.log('Got a nonce: ' + payload.nonce);

                    $.ajax({
                        type: "POST",
                        url: 'http://localhost:5000/api/braintree/ProcessPayment',
                        data: data,
                        success: callback,
                        headers: {
                            'Content-Type': 'application/json'
                        },
                    });

                    function callback(data, textStatus, jqXHR) {
                        if (jqXHR.status === 200) {
                            console.log(data);
                            alert("Payment Successful");
                            window.location.reload(true);
                        }
                    };
                });
            };

            form.addEventListener('submit', teardown, false);
        });
    }
})();