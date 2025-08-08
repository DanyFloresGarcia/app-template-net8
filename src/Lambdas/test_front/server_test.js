// server.js
const express = require('express');
const axios = require('axios');
const bodyParser = require('body-parser');
const cors = require('cors');

const app = express();
const port = 3000;
app.use(cors()); // 👈 Importante

// Middleware para permitir JSON en el body
app.use(bodyParser.json());

// Proxy hacia tu API de AWS
app.post('/api/proxy', async (req, res) => {
  try {
    const awsApiUrl = 'https://5hq4lloz27.execute-api.us-west-2.amazonaws.com/staging/login'; // Cambia esto

    const response = await axios.post(awsApiUrl, req.body, {
        resource: '/login',
        path: '/login',
        httpMethod: 'POST',
        headers: req.headers,
        body: JSON.stringify(req.body),
        isBase64Encoded: false
    },{
      headers: {
        'Content-Type': 'application/json',
        // Puedes agregar más headers si necesitas autorización
        // 'x-api-key': 'tu-api-key',
      }
    });

    res.status(response.status).json(response.data);
  } catch (error) {
    console.error('Error al llamar a la API de AWS:', error.message);
    res.status(error.response?.status || 500).json({
      error: 'Error al llamar a la API de AWS',
      detail: error.response?.data || error.message
    });
  }
});

app.listen(port, () => {
  console.log(`Proxy escuchando en http://localhost:${port}`);
});
