import './App.css';
import VehicleMakeListPage from './pages/VehicleMakeListPage';
import VehicleModelListPage from './pages/VehicleModelListPage';
import React, { useState } from 'react';
import Navigation from './components/Navigation';
import { Container } from 'react-bootstrap';

function App() {
  const [currentView, setCurrentView] = useState('makes');

  const handleViewSelect = (view) => {
      setCurrentView(view);
  };

  return (
      <Container style={{ padding: '20px' }}>
          <Navigation onSelectView={handleViewSelect} currentView={currentView} />
          {currentView === 'makes' && <VehicleMakeListPage />}
          {currentView === 'models' && <VehicleModelListPage />}
      </Container>
  );
}

export default App;
