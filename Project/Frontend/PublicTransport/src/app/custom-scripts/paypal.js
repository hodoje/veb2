function initPaypalButton(userEmail) {
    paypal.Button.render({
        // Configure environment
        env: 'sandbox',
        client: {
          sandbox: 'ATqWgyHRGxbDONKljsKFO-QIriqvHtHw_-_ehG9rq55Bdx6uDDKXB0E79REBPD1ks5bJHKzmbvwuR9M0',
          production: 'XXXX'
        },
        // Customize button (optional)
        locale: 'en_US',
        style: {
            layout: 'horizontal',
            color: 'blue',
            shape: 'rect',
            label: 'paypal',
            size: 'responsive',
            tagline: false
        },
    
        // Enable Pay Now checkout flow (optional)
        commit: true,
    
        // Set up a payment
        payment: function(data, actions) {
          var price = document.getElementById("priceInput").value;
          var ticketTypeName = document.getElementById("ticketTypeNameInput").value;

          return actions.payment.create({
            transactions: [{
              amount: {
                total: +price / 100, // Price is in RSD, approx. conversion to USD
                currency: 'USD'
              },
              item_list: {
                items: [
                  {
                    name: "bus ticket",
                    description: ticketTypeName + " " + "ticket",
                    quantity : "1",
                    price: +price / 100, // Price is in RSD, approx. conversion to USD
                    currency: "USD"
                  }
                ]
              }
            }]
          });
        },
        // Execute the payment
        onAuthorize: function(data, actions) {
          return actions.payment.execute().then(function(response) {
            var ticketType = document.getElementById("ticketTypeInput").value;
            var email = document.getElementById("email").value;

            var emailToSet = null;
            if(!email){
              emailToSet = userEmail;
            }
            else{
              emailToSet = email;
            }

            var customResponse = {
              userEmail: emailToSet,
              transaction: {
                orderId: response.transactions[0].related_resources[0].sale.id,
                dateCreated: response.create_time,
                status: response.transactions[0].related_resources[0].sale.state,
                payerEmail: response.payer.payer_info.email,
                price: response.transactions[0].amount.total,
                currency: response.transactions[0].amount.currency
              },
              ticketDto: {
                ticketTypeId: ticketType,
                email: emailToSet
              }
            }
            
            var clearPurchaseBtn = document.getElementById("clearPurchaseBtn");
            clearPurchaseBtn.click();

            return fetch('http://localhost:52296/api/Tickets/PaypalPurchase', {
                          method: 'post',
                          headers: {
                              'content-type': 'application/json',
                          },
                          body: JSON.stringify(customResponse)
                      }).then((response) => {
                        if(response.status === 200){
                          var showPositiveFeedbackBtn = document.getElementById("showPositiveFeedbackBtn");
                          showPositiveFeedbackBtn.click();
                        }
                        else{
                          var showNegativeFeedbackBtn = document.getElementById("showNegativeFeedbackBtn");
                          showNegativeFeedbackBtn.click();  
                        }
                      }, (error) => {
                        var showNegativeFeedbackBtn = document.getElementById("showNegativeFeedbackBtn");
                        showNegativeFeedbackBtn.click();
                      }).catch((error) => {
                        var showNegativeFeedbackBtn = document.getElementById("showNegativeFeedbackBtn");
                        showNegativeFeedbackBtn.click();
                      });
          });
        }
      }, '#paypal-button');
}