import { useState } from 'react'

type SearchActorsProps = {
  onSearch: (query: string) => void
}

export const SearchActors = ({ onSearch }: SearchActorsProps) => {
  const [query, setQuery] = useState('')

  const handleSearch = () => {
    onSearch(query)
  }

  const handleKeyPress = (e: { key: string }) => {
    if (e.key === 'Enter') {
      handleSearch()
    }
  }

  const clear = () => {
    setQuery('')
    handleSearch()
  }

  return (
    <div className="flex mb-4">
      <input
        type="text"
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        onKeyPress={handleKeyPress}
        placeholder="Search actors..."
        className="border p-2 rounded"
      />
      <button onClick={handleSearch} className="ml-2 p-2 bg-blue-500 text-white rounded">
        Search
      </button>
      <button onClick={clear} className="ml-2 p-2 bg-red-500 text-white rounded">
        Clear
      </button>
    </div>
  )
}
