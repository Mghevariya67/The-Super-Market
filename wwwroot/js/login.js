const signUpButton = document.getElementById('signUp');
const signInButton = document.getElementById('signIn');
const container = document.getElementById('container');



signUpButton.addEventListener('click', () => {
	container.classList.add("right-panel-active");
});

signInButton.addEventListener('click', () => {
	container.classList.remove("right-panel-active");
});

function signIn(){	
	var email = document.getElementById('email').value;
	var pwd = document.getElementById('pwd').value;

	console.log('Email:'+email +', Password:'+pwd);
	if(email == 'vivek@gmail.com' && pwd === 'shah0510') {
	alert("signin");
		location.href = "The Indian SuperMarket_Admin/index.html";
	
	}
	else if(email === "mitesh@gmail.com" && pwd === "123456") {
		location.href= "The Indian SuperMarket/index.html";
		return false;
	}
	else{
		window.location = "The Indian SuperMarket/index.html";
	}

}

