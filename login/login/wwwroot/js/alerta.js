
var email;
var phoneNumber;
var passwordHash;
var role;
var selectRole;

function back(){
    window.location.replace("../");
}


    $.ajax({
        type: "POST",
        url: action,
        data: {},
        succes: function (response)
        {
            if (j == 0)
            {
                for (var i = 0; i < response.length; i++)
                {
                    document.getElementById("select").options[i] = new Option(response[i].text, response[i].value);
                    document.getElementById("selectNuevo").options[i] = new Option(response[i].text, response[i].value);
                }
            }
            j = 1;
        }

    });
   

function crearUsuario(action)
{
    
   //obtener los datos ingregresados en los input
    email = $('input[name=EmailNuevo]')[0].value;
    phoneNumber = $('input[name=Celular]')[0].value;
    passwordHash = $('input[name=PasswordHashNuevo]')[0].value;
    role = document.getElementById('selectNuevo');
    selectRole = role.options[role.selectedIndex].text;
    

    //validar que los datos no esten llenos
    if (email === "") {
        $('#EmailNuevo').focus();
        alert("Ingrese el Correo Electronico");
    } else
    {
        if (passwordHash === "") {
            $('#PasswordHashNuevo').focus();
            alert("Ingrese la contrasena");
        } else
        {
            $.ajax
                ({
                    type:"POST",
                    url:action,
                    data:
                        {
                            email, phoneNumber, passwordHash, selectRole
                        },
                    succes: function (response)
                    {
                        if (response === "save") {
                            window.location.replace("../");
                        } else
                        {
                            $('#men-new').html("No se puede agregar el Usuaio</br>selecciones un rol</br> Increse un correo electronico Correcto");
                        }
                    }
            });
        }
    }
}