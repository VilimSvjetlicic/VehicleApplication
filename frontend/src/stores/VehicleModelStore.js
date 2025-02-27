import { makeAutoObservable, runInAction } from 'mobx';

const API_BASE = 'https://localhost:7224/api/VehicleModel';

class VehicleModelStore {
    vehicleModels = [];
    filterText = "";
    selectedMakeFilterId = null;
    sortColumn = null;
    sortDirection = 'asc';
    currentPage = 1;
    pageSize = 5;
    loading = false;
    error = null;

    constructor() {
        makeAutoObservable(this);
        this.fetchVehicleModels();
    }

    async fetchVehicleModels() {
        this.loading = true;
        this.error = null;
        try {
            const response = await fetch(API_BASE);
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            runInAction(() => {
                this.vehicleModels = data;
                this.loading = false;
            });
        } catch (error) {
            runInAction(() => {
                this.error = error;
                this.loading = false;
            });
            console.error("Error fetching vehicle models:", error);
        }
    }

    get filteredVehicleModels() {
        let models = this.vehicleModels;
        if (this.selectedMakeFilterId) {
            models = models.filter(model => model.makeId === this.selectedMakeFilterId);
        }
        const filterRegex = new RegExp(this.filterText, 'i');
        return models.filter(model =>
            filterRegex.test(model.name) || filterRegex.test(model.abrv)
        );
    }

    get sortedVehicleModels() {
        if (!this.sortColumn) {
            return this.filteredVehicleModels;
        }
        const sorted = [...this.filteredVehicleModels].sort((a, b) => {
            const aValue = a[this.sortColumn] || '';
            const bValue = b[this.sortColumn] || '';
            if (typeof aValue === 'string' && typeof bValue === 'string') {
                return this.sortDirection === 'asc' ? aValue.localeCompare(bValue) : bValue.localeCompare(bValue);
            }
            return this.sortDirection === 'asc' ? (aValue > bValue ? 1 : -1) : (bValue > aValue ? 1 : -1);
        });
        return sorted;
    }

    get pagedVehicleModels() {
        const startIndex = (this.currentPage - 1) * this.pageSize;
        return this.sortedVehicleModels.slice(startIndex, startIndex + this.pageSize);
    }

    get totalPages() {
        return Math.ceil(this.sortedVehicleModels.length / this.pageSize);
    }

    async addVehicleModel(model) {
        try {
            const response = await fetch(API_BASE, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(model)
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            runInAction(() => {
                this.vehicleModels.push(data);
            });
        } catch (error) {
            console.error("Error adding vehicle model:", error);
        }
    }
    
    async updateVehicleModel(updatedModel) {
        try {
            const response = await fetch(`${API_BASE}/${updatedModel.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(updatedModel)
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            runInAction(() => {
                this.vehicleModels = this.vehicleModels.map(model =>
                    model.id === updatedModel.id ? updatedModel : model
                );
            });
        } catch (error) {
            console.error("Error updating vehicle model:", error);
        }
    }
    
    async deleteVehicleModel(id) {
        try {
            const response = await fetch(`${API_BASE}/${id}`, {
                method: 'DELETE'
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            runInAction(() => {
                this.vehicleModels = this.vehicleModels.filter(model => model.id !== id);
            });
        } catch (error) {
            console.error("Error deleting vehicle model:", error);
        }
    }

    setFilterText = (text) => {
        this.filterText = text;
        this.currentPage = 1;
    };

    setSelectedMakeFilterId = (makeId) => {
        this.selectedMakeFilterId = makeId;
        this.currentPage = 1;
    };

    setSortColumn = (column) => {
        if (this.sortColumn === column) {
            this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            this.sortColumn = column;
            this.sortDirection = 'asc';
        }
    };

    setCurrentPage = (page) => {
        this.currentPage = page;
    };

    setPageSize = (size) => {
        this.pageSize = size;
        this.currentPage = 1;
    };

    getVehicleModelById = (id) => {
        return this.vehicleModels.find(model => model.id === id);
    };

    getModelsByMake = (makeId) => {
        return this.vehicleModels.filter(model => model.makeId === makeId);
    };
}

export const vehicleModelStore = new VehicleModelStore();