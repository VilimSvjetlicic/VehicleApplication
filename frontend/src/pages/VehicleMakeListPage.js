import React, { useState } from 'react';
import { observer } from 'mobx-react-lite';
import { vehicleMakeStore } from '../stores/VehicleMakeStore.js';
import DataTable from '../components/DataTable.js';
import VehicleMakeForm from '../forms/VehicleMakeForm.js';
import Filter from '../components/Filter.js';
import Button from 'react-bootstrap/Button';
import ConfirmationModal from '../components/ConfirmModal.js';
import { Container, Row, Col } from 'react-bootstrap';

const VehicleMakeListPage = observer(() => {
    const [editingMakeId, setEditingMakeId] = useState(null);
    const [isAddingNew, setIsAddingNew] = useState(false);
    const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
    const [makeToDeleteId, setMakeToDeleteId] = useState(null);
    const [showFormModal, setShowFormModal] = useState(false);

    const handleEdit = (id) => {
        setEditingMakeId(id);
        setIsAddingNew(false);
        setShowFormModal(true);
    };

    const handleCancelEdit = () => {
        setEditingMakeId(null);
        setIsAddingNew(false);
        setShowFormModal(false);
    };

    const handleDelete = (id) => {
        setMakeToDeleteId(id);
        setShowDeleteConfirmation(true);
    };

    const confirmDelete = () => {
        vehicleMakeStore.deleteVehicleMake(makeToDeleteId);
        setShowDeleteConfirmation(false);
        setMakeToDeleteId(null);
    };

    const cancelDelete = () => {
        setShowDeleteConfirmation(false);
        setMakeToDeleteId(null);
    };


    const handleSave = () => {
        setEditingMakeId(null);
        setIsAddingNew(false);
        setShowFormModal(false);
    };

    const handleAddNew = () => {
        setIsAddingNew(true);
        setEditingMakeId(null);
        setShowFormModal(true);
    };


    const columns = [
        { header: 'Name', field: 'name', sortable: true },
        { header: 'Abbreviation', field: 'abrv', sortable: true },
    ];

    const renderActions = (make) => (
        <>
            <Button variant="primary" size="sm" onClick={() => handleEdit(make.id)} className="me-2">Edit</Button>
            <Button variant="danger" size="sm" onClick={() => handleDelete(make.id)}>Delete</Button>
        </>
    );

    return (
        <Container fluid>
            <Row>
                <Col>
                    <h1>Vehicle Makes</h1>
                </Col>
            </Row>
            <Row className="mb-3">
                <Col>
                    <Filter
                        filterText={vehicleMakeStore.filterText}
                        onFilterChange={vehicleMakeStore.setFilterText}
                        placeholder="Filter by name or abbreviation"
                    />
                </Col>
                <Col md="auto">
                    <Button variant="success" onClick={handleAddNew}>Add New Vehicle Make</Button>
                </Col>
            </Row>
            <Row>
                <Col>
                    <DataTable
                        data={vehicleMakeStore.pagedVehicleMakes}
                        columns={columns}
                        sortColumn={vehicleMakeStore.sortColumn}
                        sortDirection={vehicleMakeStore.sortDirection}
                        onSort={vehicleMakeStore.setSortColumn}
                        currentPage={vehicleMakeStore.currentPage}
                        totalPages={vehicleMakeStore.totalPages}
                        pageSize={vehicleMakeStore.pageSize}
                        onPageChange={vehicleMakeStore.setCurrentPage}
                        onPageSizeChange={vehicleMakeStore.setPageSize}
                        renderActions={renderActions}
                    />
                </Col>
            </Row>

            <ConfirmationModal
                show={showFormModal}
                onHide={handleCancelEdit}
                title={isAddingNew ? "Add New Vehicle Make" : "Edit Vehicle Make"}
                body={
                    <VehicleMakeForm
                        makeId={editingMakeId}
                        onSave={handleSave}
                        onCancel={handleCancelEdit}
                        store={vehicleMakeStore}
                    />
                }
                onConfirm={null}
                cancelText="Close"
            />

            <ConfirmationModal
                show={showDeleteConfirmation}
                onHide={cancelDelete}
                title="Delete Confirmation"
                body="Are you sure you want to delete this Vehicle Make?"
                onConfirm={confirmDelete}
                onCancel={cancelDelete}
                confirmText="Delete"
                cancelText="Cancel"
                isConfirmation={true}
            />
        </Container>
    );
});

export default VehicleMakeListPage;