// chat.js
const token = localStorage.getItem('token');
if (token) {
    localStorage.setItem('token', token); // Almacena el token en el navegador
    console.log("Token almacenado:", token); // Verifica que el token se ha almacenado
}
fetch('/Chat', {
    method: 'POST',
    headers: {
        'Authorization': 'Bearer ' + token
    }
})
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al cargar el chat. Verifique su autenticación.');
        }
        return response.text();
    })
    .then(data => {
        console.log(data); // Mostrar o procesar los datos del chat
    })
    .catch(error => {
        console.error('Error:', error);
    });
