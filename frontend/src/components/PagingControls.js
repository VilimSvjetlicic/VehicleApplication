import React from 'react';
import Pagination from 'react-bootstrap/Pagination';
import Form from 'react-bootstrap/Form';

const PagingControls = ({ currentPage, totalPages, pageSize, onPageChange, onPageSizeChange }) => {
    let paginationItems = [];
    for (let number = 1; number <= totalPages; number++) {
        paginationItems.push(
            <Pagination.Item key={number} active={number === currentPage} onClick={() => onPageChange(number)}>
                {number}
            </Pagination.Item>,
        );
    }

    return (
        <div className="d-flex align-items-center">
            <Pagination size="sm">{paginationItems}</Pagination>
            <Form.Select
                value={pageSize}
                onChange={(e) => onPageSizeChange(Number(e.target.value))}
                className="ms-3"
                style={{ width: 'auto' }}
            >
                <option value={5}>5</option>
                <option value={10}>10</option>
            </Form.Select>
            <span className="ms-2">Page Size</span>
        </div>
    );
};

export default PagingControls;