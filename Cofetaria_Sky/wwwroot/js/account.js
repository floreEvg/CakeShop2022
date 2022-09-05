function register() {

    let registerPayload = {
        FirstName: document.getElementById("prenume").value,
        LastName: document.getElementById("nume").value,
        Email: document.getElementById("email_reg").value,
        Phone: document.getElementById("phone").value,
        Address: document.getElementById("address").value,
        Password: document.getElementById("parola_reg").value,
    };

    data = JSON.stringify(registerPayload);
    /*
    if (registerPayload.FirstName.length != 0 &&
        registerPayload.LastName.length != 0 &&
        registerPayload.Email.length != 0 &&
        registerPayload.Password.length != 0 &&
        registerPayload.ConfirmPassword.length != 0 &&
        registerPayload.Phone.length != 0 &&
        registerPayload.Address.length != 0) {
        
        let xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {

            if (this.readyState == 4 && this.status == 200) {
                console.log(this.responseText);

            }
            if (this.readyState == 4 && this.status == 404) {
                console.log(this.responseText);

            }
        };

        
        xhr.open("POST", "http://localhost:44795/api/account/register", true);
        xhr.setRequestHeader('Access-Control-Allow-Origin', 'http://localhost:44367');
        xhr.setRequestHeader('Access-Control-Allow-Methods', 'GET, POST');
        http.setRequestHeader('Access-Control-Allow-Headers', 'Content-Type')

        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.send(JSON.stringify(registerPayload));
        console.log(xhr.status);
       
   }
   */

    $.ajax({
        type: "POST",
        url: "http://localhost:44795/api/account/register",
        dataType: "json",
        crossDomain: true,
        
        success: function (data) {
            console.log("a mers");

        },

        error: function () {
            console.log("fisierul nu exista");
        }
    });
}