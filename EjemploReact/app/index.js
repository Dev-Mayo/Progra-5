import React, { useEffect, useState } from 'react';
import { SafeAreaView, View, Text, TextInput, Button, StyleSheet, ActivityIndicator } from 'react-native';

const API_KEY = 'c25952bedaec4b23b0c45145260804 ';

export default function App() {
  const [city, setCity] = useState('');
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // Función para obtener el clima
  const obtenerClima = async () => {
    if (!city) return;

    setLoading(true);
    setError('');

    try {
      const response = await fetch(`https://api.weatherapi.com/v1/current.json?key=${API_KEY}&q=${city}&lang=es`);
      
      if (!response.ok) {
        throw new Error('No se pudo obtener la información del clima');
      }

      const data = await response.json();
      setWeather(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  // Función para renderizar los datos del clima
  const renderWeather = () => {
    if (!weather) return null;

    return (
      <View style={styles.weatherCard}>
        <Text style={styles.city}>{weather.location.name}, {weather.location.country}</Text>
        <Text style={styles.temp}>{weather.current.temp_c}°C</Text>
        <Text style={styles.description}>{weather.current.condition.text}</Text>
      </View>
    );
  };

  return (
    <SafeAreaView style={styles.container}>
      <Text style={styles.header}>Consulta el clima</Text>

      <TextInput
        style={styles.input}
        placeholder="Ingresa una ciudad"
        value={city}
        onChangeText={setCity}
      />
      <Button title="Buscar clima" onPress={obtenerClima} />

      {loading && <ActivityIndicator size="large" style={styles.loader} />}
      
      {error ? <Text style={styles.error}>{error}</Text> : renderWeather()}
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    padding: 16,
    backgroundColor: '#fff',
  },
  header: {
    fontSize: 22,
    fontWeight: 'bold',
    marginBottom: 16,
    textAlign: 'center',
  },
  input: {
    height: 40,
    borderColor: '#ccc',
    borderWidth: 1,
    borderRadius: 5,
    marginBottom: 12,
    paddingLeft: 8,
  },
  loader: {
    marginTop: 20,
  },
  weatherCard: {
    marginTop: 20,
    padding: 16,
    backgroundColor: '#f4f4f4',
    borderRadius: 10,
    alignItems: 'center',
  },
  city: {
    fontSize: 20,
    fontWeight: 'bold',
  },
  temp: {
    fontSize: 36,
    fontWeight: 'bold',
    marginVertical: 10,
  },
  description: {
    fontSize: 18,
    color: '#555',
  },
  error: {
    color: 'red',
    textAlign: 'center',
    marginTop: 10,
  },
});