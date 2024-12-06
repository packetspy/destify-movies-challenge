import { useQueryClient } from '@tanstack/react-query'
import { useSearchParams } from 'react-router-dom'
import { useState } from 'react'

import { Link } from '@/components/ui/link'
import { Spinner } from '@/components/ui/spinner'
import { Table } from '@/components/ui/table'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'

import { paths } from '@/config/paths'
import { getDirectorQueryOptions } from '../api/get-director'
import { useDirectors } from '../api/get-directors'
import { DeleteDirector } from './delete-director'
import { SearchDirectors } from './directors-search'
import { Eye } from 'lucide-react'

export type DirectorsListProps = {
  onDirectorPrefetch?: (id: string) => void
}

export const DirectorsList = ({ onDirectorPrefetch }: DirectorsListProps) => {
  const [searchParams] = useSearchParams()
  const [searchQuery, setSearchQuery] = useState('')

  const query = useDirectors({
    query: searchQuery,
    page: +(searchParams.get('page') || 1)
  })

  const queryClient = useQueryClient()

  if (query.isLoading) {
    return (
      <div className="flex h-48 w-full items-center justify-center">
        <Spinner size="lg" />
      </div>
    )
  }

  const Directors = query.data?.data
  const meta = query.data?.meta

  if (!Directors) return null

  const handleSearch = (query: string) => {
    if (query !== '' || query != undefined) setSearchQuery(query)
  }

  return (
    <>
      <SearchDirectors onSearch={handleSearch} />
      <Separator className="my-4" />
      <Table
        data={Directors}
        columns={[
          {
            title: 'Name',
            field: 'name',
            Cell({ entry: { name } }) {
              return <div className="w-96">{name}</div>
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
                      queryClient.prefetchQuery(getDirectorQueryOptions(uniqueId))
                      onDirectorPrefetch?.(uniqueId)
                    }}
                    to={paths.app.director.getHref(uniqueId)}
                  >
                    <Button>
                      <Eye className="size-4" />
                    </Button>
                  </Link>

                  <DeleteDirector uniqueId={uniqueId} />
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
