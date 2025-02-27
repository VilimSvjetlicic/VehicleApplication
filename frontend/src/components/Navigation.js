import React from 'react';
import { Navbar, Nav } from 'react-bootstrap';

const Navigation = ({ onSelectView, currentView }) => {
    return (
        <Navbar bg="light" expand="lg">
            <Navbar.Brand href="#home">Vehicle App</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
                <Nav className="me-auto">
                    <Nav.Link
                        onClick={() => onSelectView('makes')}
                        active={currentView === 'makes'}
                    >
                        Vehicle Makes
                    </Nav.Link>
                    <Nav.Link
                        onClick={() => onSelectView('models')}
                        active={currentView === 'models'}
                    >
                        Vehicle Models
                    </Nav.Link>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    );
};

export default Navigation;