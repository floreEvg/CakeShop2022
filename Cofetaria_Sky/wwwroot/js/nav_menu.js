function menu() {
    if (document.getElementById("buton").style.color === "red") {
        document.getElementById("buton").style.color = "black";

        //pagina principala
        if (document.getElementById("carousel")) {
            document.getElementById("carousel").style.marginTop = "210px";
        }

        //cont
        else if (document.getElementById("denied")) {
            document.getElementById("denied").style.marginTop = "300px";
        }
        else if (document.getElementById("change_email")) {
            document.getElementById("change_email").style.marginTop = "350px";
        }
        else if (document.getElementById("change_password")) {
            document.getElementById("change_password").style.marginTop = "300px";
        }
        else if (document.getElementById("reset")) {
            document.getElementById("reset").style.marginTop = "210px";
        }
        else if (document.getElementById("comenzi")) {
            document.getElementById("comenzi").style.marginTop = "250px";
        }
        else if (document.getElementById("cont")) {
            document.getElementById("cont").style.marginTop = "210px";
        }
        else if (document.getElementById("detalii_comanda")) {
            document.getElementById("detalii_comanda").style.marginTop = "250px";
        }
        else if (document.getElementById("forgot")) {
            document.getElementById("forgot").style.marginTop = "300px";
        }
        else if (document.getElementById("autentificare")) {
            document.getElementById("autentificare").style.marginTop = "210px";
        }
        else if (document.getElementById("register")) {
            document.getElementById("register").style.marginTop = "250px";
        }
        else if (document.getElementById("update")) {
            document.getElementById("update").style.marginTop = "210px";
        }

        //produse
        else if (document.getElementById("add_product")) {
            document.getElementById("add_product").style.marginTop = "210px";
        }
        else if (document.getElementById("delete_product")) {
            document.getElementById("delete_product").style.marginTop = "250px";
        }
        else if (document.getElementById("patiserie")) {
            document.getElementById("patiserie").style.marginTop = "250px";
        }
        else if (document.getElementById("prajitura")) {
            document.getElementById("prajitura").style.marginTop = "250px";
        }
        else if (document.getElementById("titlu_produs")) {
            document.getElementById("titlu_produs").style.marginTop = "250px";
        }
        else if (document.getElementById("produse")) {
            document.getElementById("produse").style.marginTop = "300px";
        }
        else if (document.getElementById("tort")) {
            document.getElementById("tort").style.marginTop = "250px";
        }
        else if (document.getElementById("update_product")) {
            document.getElementById("update_product").style.marginTop = "250px";
        }
        
        //cos
        else if (document.getElementById("finalizare")) {
            document.getElementById("finalizare").style.marginTop = "250px";
        }
        else if (document.getElementById("cos")) {
            document.getElementById("cos").style.marginTop = "250px";
        }

    } else {
        document.getElementById("buton").style.color = "red";

        //pagina principala
        if (document.getElementById("carousel")) {
            document.getElementById("carousel").style.marginTop = "76px";
        }

        //cont
        else if (document.getElementById("denied")) {
            document.getElementById("denied").style.marginTop = "76px";
        }
        else if (document.getElementById("change_email")) {
            document.getElementById("change_email").style.marginTop = "76px";
        }
        else if (document.getElementById("change_password")) {
            document.getElementById("change_password").style.marginTop = "76px";
        }
        else if (document.getElementById("reset")) {
            document.getElementById("reset").style.marginTop = "76px";
        }
        else if (document.getElementById("comenzi")) {
            document.getElementById("comenzi").style.marginTop = "76px";
        }
        else if (document.getElementById("cont")) {
            document.getElementById("cont").style.marginTop = "76px";
        }
        else if (document.getElementById("detalii_comanda")) {
            document.getElementById("detalii_comanda").style.marginTop = "76px";
        }
        else if (document.getElementById("forgot")) {
            document.getElementById("forgot").style.marginTop = "76px";
        }
        else if (document.getElementById("autentificare")) {
            document.getElementById("autentificare").style.marginTop = "76px";
        }
        else if (document.getElementById("register")) {
            document.getElementById("register").style.marginTop = "76px";
        }
        else if (document.getElementById("update")) {
            document.getElementById("update").style.marginTop = "76px";
        }

        //produse
        else if (document.getElementById("add_product")) {
            document.getElementById("add_product").style.marginTop = "0px";
        }
        else if (document.getElementById("delete_product")) {
            document.getElementById("delete_product").style.marginTop = "0px";
        }
        else if (document.getElementById("patiserie")) {
            document.getElementById("patiserie").style.marginTop = "120px";
        }
        else if (document.getElementById("prajitura")) {
            document.getElementById("prajitura").style.marginTop = "120px";
        }
        else if (document.getElementById("titlu_produs")) {
            document.getElementById("titlu_produs").style.marginTop = "100px";
        }
        else if (document.getElementById("produse")) {
            document.getElementById("produse").style.marginTop = "150px";
        }
        else if (document.getElementById("tort")) {
            document.getElementById("tort").style.marginTop = "120px";
        }
        else if (document.getElementById("update_product")) {
            document.getElementById("update_product").style.marginTop = "0px";
        }

        //cos
        else if (document.getElementById("finalizare")) {
            document.getElementById("finalizare").style.marginTop = "100px";
        }
        else if (document.getElementById("cos")) {
            document.getElementById("cos").style.marginTop = "90px";
        }
    }
}