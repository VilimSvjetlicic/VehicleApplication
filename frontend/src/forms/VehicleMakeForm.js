import React, { useState, useEffect } from 'react';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

const VehicleMakeForm = ({ makeId, onSave, onCancel, store }) => {
    const [name, setName] = useState('');
    const [abrv, setAbrv] = useState('');
    const [error, setError] = useState('');

    useEffect(() => {
        if (makeId) {
            const existingMake = store.getVehicleMakeById(makeId);
            if (existingMake) {
                setName(existingMake.name);
                setAbrv(existingMake.abrv);
            }
        } else {
            setName('');
            setAbrv('');
        }
    }, [makeId, store]);

    const handleSubmit = (e) => {
        e.preventDefault();
        if (!name || !abrv) {
            setError('Name and Abbreviation are required.');
            return;
        }
        setError('');

        const makeData = { name, abrv };
        if (makeId) {
            store.updateVehicleMake({ id: makeId, ...makeData });
        } else {
            store.addVehicleMake(makeData);
        }
        onSave();
    };

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Group className="mb-3">
                <Form.Label htmlFor="name">Name:</Form.Label>
                <Form.Control type="text" id="name" value={name} onChange={(e) => setName(e.target.value)} />
            </Form.Group>
            <Form.Group className="mb-3">
                <Form.Label htmlFor="abrv">Abbreviation:</Form.Label>
                <Form.Control type="text" id="abrv" value={abrv} onChange={(e) => setAbrv(e.target.value)} />
            </Form.Group>
            <div className="d-flex justify-content-end">
                {error && <p style={{ color: 'red', marginRight: 10}}>{error}</p>}
                <Button variant="primary" type="submit" className="me-2">Save</Button>
                <Button variant="secondary" onClick={onCancel}>Cancel</Button>
            </div>
            
        </Form>
    );
};

export default VehicleMakeForm;