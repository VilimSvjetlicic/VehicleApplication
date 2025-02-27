import React from 'react';
import Form from 'react-bootstrap/Form';

const Filter = ({ filterText, onFilterChange, placeholder }) => {
    return (
        <Form.Control
            type="text"
            placeholder={placeholder}
            value={filterText}
            onChange={(e) => onFilterChange(e.target.value)}
            className="mb-3"
        />
    );
};

export default Filter;