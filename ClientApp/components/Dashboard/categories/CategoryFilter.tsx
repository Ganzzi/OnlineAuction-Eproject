interface CategoryFilterProps {
    search: string;
    belongToCategory: string | null;
    onSearchChange: (value: string) => void;
    onSelectChange: (value: string) => void;
  }
  
  const CategoryFilter: React.FC<CategoryFilterProps> = ({ search, belongToCategory, onSearchChange, onSelectChange }) => {
    return (
      <div className="flex space-x-4 mb-4">
        {/* Search input */}
        <input
          type="text"
          value={search}
          onChange={(e) => onSearchChange(e.target.value)}
          placeholder="Search"
          className="border p-2"
        />
  
        {/* Select dropdown */}
        <select value={belongToCategory || ''} onChange={(e) => onSelectChange(e.target.value)} className="border p-2">
          <option value="">All Categories</option>
          <option value="true">Belonging to Category</option>
          <option value="false">Not Belonging to Category</option>
        </select>
      </div>
    );
  };
  
  export default CategoryFilter;