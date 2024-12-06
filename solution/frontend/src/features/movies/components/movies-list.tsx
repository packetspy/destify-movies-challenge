import { useQueryClient } from '@tanstack/react-query'
import { useSearchParams } from 'react-router-dom'
import { useState } from 'react'

import { Link } from '@/components/ui/link'
import { Spinner } from '@/components/ui/spinner'
import { Table } from '@/components/ui/table'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'

import { paths } from '@/config/paths'
import { getMovieQueryOptions } from '../api/get-movie'
import { useMovies } from '../api/get-movies'
import { DeleteMovie } from './delete-movie'
import { SearchMovies } from './movies-search'
import { Eye } from 'lucide-react'

export type MoviesListProps = {
  onMoviePrefetch?: (id: string) => void
}

export const MoviesList = ({ onMoviePrefetch }: MoviesListProps) => {
  const [searchParams] = useSearchParams()
  const [searchQuery, setSearchQuery] = useState('')

  const moviesQuery = useMovies({
    query: searchQuery,
    page: +(searchParams.get('page') || 1)
  })

  const queryClient = useQueryClient()

  if (moviesQuery.isLoading) {
    return (
      <div className="flex h-48 w-full items-center justify-center">
        <Spinner size="lg" />
      </div>
    )
  }

  const movies = moviesQuery.data?.data
  const meta = moviesQuery.data?.meta

  if (!movies) return null

  const handleSearch = (query: string) => {
    if (query !== '' || query != undefined) setSearchQuery(query)
  }

  return (
    <>
      <SearchMovies onSearch={handleSearch} />
      <Separator className="my-4" />
      <Table
        data={movies}
        columns={[
          {
            title: 'Title',
            field: 'title',
            Cell({ entry: { title } }) {
              return <div className="w-96">{title}</div>
            }
          },
          {
            title: 'Actions',
            field: 'uniqueId',
            Cell({ entry: { uniqueId } }) {
              return (
                <>
                  <Link
                    className="mr-2"
                    onMouseEnter={() => {
                      queryClient.prefetchQuery(getMovieQueryOptions(uniqueId))
                      onMoviePrefetch?.(uniqueId)
                    }}
                    to={paths.app.movie.getHref(uniqueId)}
                  >
                    <Button>
                      <Eye className="size-4" />
                    </Button>
                  </Link>

                  <DeleteMovie movieUniqueId={uniqueId} />
                </>
              )
            }
          }
        ]}
        pagination={
          meta && {
            totalPages: meta.totalPages,
            currentPage: meta.page,
            rootUrl: ''
          }
        }
      />
    </>
  )
}
