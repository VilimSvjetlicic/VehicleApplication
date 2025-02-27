import React, { useState, useEffect } from 'react';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

const VehicleModelForm = ({ modelId, onSave, onCancel, makeStore, modelStore }) => {
    const [name, setName] = useState('');
    const [abrv, setAbrv] = useState('');
    const [makeId, setMakeId] = useState(null);
    const [error, setError] = useState('');

    useEffect(() => {
        if (modelId) {
            const existingModel = modelStore.getVehicleModelById(modelId);
            if (existingModel) {
                setName(existingModel.name);
                setAbrv(existingModel.abrv);
                setMakeId(existingModel.makeId);
            }
        } else {
            setName('');
            setAbrv('');
            setMakeId(null);
        }
    }, [modelId, modelStore]);


    const handleSubmit = (e) => {
        e.preventDefault();
        if (!name || !abrv || !makeId) {
            setError('Name, Abbreviation and Make are required.');
            return;
        }
        setError('');

        const modelData = { name, abrv, makeId };
        if (modelId) {
            modelStore.updateVehicleModel({ id: modelId, ...modelData });
        } else {
            modelStore.addVehicleModel(modelData);
        }
        onSave();
    };

    return (
        <Form onSubmit={handleSubmit}>
            <Form.Group className="mb-3">
                <Form.Label htmlFor="makeId">Make:</Form.Label>
                <Form.Select id="makeId" value={makeId || ''} onChange={(e) => setMakeId(Number(e.target.value))}>
                    <option value="">Select Make</option>
                    {makeStore.vehicleMakes.map(make => ( // Use makeStore prop
                        <option key={make.id} value={make.id}>{make.name}</option>
                    ))}
                </Form.Select>
            </Form.Group>
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

export default VehicleModelForm;