var reload = document.getElementById("chat-body").innerHTML;
var sessionId ="";
function openForm() {
    document.getElementById("myForm").style.display = "block";
  }
  
  function closeForm() {
    sessionId = null;
    document.getElementById("myForm").style.display = "none";
  }

  function sendMessage() {
    var message = document.getElementById("status_message").value;
    document.getElementById("chat-body").insertAdjacentHTML('beforeend',
      '<div class="container darker">'+
      '<img src="images/me.jpg" alt="Avatar" class="right"  style="widows: 50px;">'+
      '<p>'+message+'</p>'+
      '<span class="time-right">'+ new Date().toLocaleTimeString('en-US', { hour12: true, 
        hour: "numeric", 
        minute: "numeric"})+'</span>'+
      '</div>'  
    )
    console.log();
    var sh1 = document.getElementById('chat-body').scrollHeight;
    $("#chat-body").animate({ scrollTop: sh1 }, 600);
    document.getElementById("status_message").value = "";

    $(document).ready(function(){
      data = {
        text: message,
        sessionId: sessionId
    }
      $.ajax({  
        type: "POST",  
        url: "http://localhost:8088/ws/api/ChatBotApi/ResponseMessageV2",   
        data: JSON.stringify(data),  
        contentType: "application/json;charset=utf-8",  
        datatype: "json",  
        success: function(responseFromServer) {
          var responce = responseFromServer[0];
          sessionId = responseFromServer[1];
          console.log(responce);
          
          document.getElementById("chat-body").insertAdjacentHTML('beforeend',
          '<div class="container">'+
          '<img src="images/netexam.png" alt="Avatar" style="width:50%;">'+
          '<p>'+responce+'</p>'+
          '<span class="time-right">'+ new Date().toLocaleTimeString('en-US', { hour12: true, 
            hour: "numeric", 
            minute: "numeric"})+'</span>'+
          '</div>'  
        )
        var sh = document.getElementById('chat-body').scrollHeight;
        $("#chat-body").animate({ scrollTop: sh }, 600);
        }  
    });  
  });
}

var input = document.getElementById("status_message");
input.addEventListener("keyup", function(event) {
  if (event.keyCode === 13) {
   event.preventDefault();
   document.getElementById("sendButton").click();
  }
});

